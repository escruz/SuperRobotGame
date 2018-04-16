using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // Controling movement and attacks for the player
    public class PlayerController : MonoBehaviour {
        
        [Header("Move Speed")]
        public float speed = 6f;
        public float speedSmoothing = 10f;
        public float turnSpeed = 10f;
        public float jumpSpeed = 16f;
        public float doubleJumpSpeed = 12f;

        [Header("Ground Checks")]
        [Tooltip("Points to use for raycasting to the ground")]
        public Transform[] groundChecks;
        public float groundCheckDistance = 0.5f;

        [Header("Skills")]
        public float attackDuration = 1f;
        public float attackForce = 2f;
        public bool canDoubleJump = false;

        [Header("Audio")]
        public AudioClip jumpClip;
        public AudioClip punchClip;

        [Header("Misc")]
        public Hitbox punchHitbox;
        
        float deadZone = 0.1f;

        Rigidbody m_Rigidbody;
        Animator m_Animator;
        CharacterStats m_CharacterStats;
        AudioSource m_AudioSource;

        Vector3 m_MoveDirection;
        Quaternion m_RotationDirection;

        float m_Horizontal;
        bool m_JumpPending;
        bool m_AttackPending;

        bool m_IsFacingRight = true;
        float m_TimerAttack = 0;
        bool m_UsedDoubleJump = false;

        //--------------------------------------------
        // Unity Methods
        //--------------------------------------------

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
            m_CharacterStats = GetComponent<CharacterStats>();
            m_AudioSource = GetComponent<AudioSource>();
        }
        
        private void Update() {
            HandleInput();
        }

        private void FixedUpdate() {
            Move();
            Jump();
            Attack();
            AnimateMovement();
        }

        //--------------------------------------------
        // Flags
        //--------------------------------------------
        bool IsAttacking() {
            return m_TimerAttack != 0;
        }

        //--------------------------------------------
        // Controls
        //--------------------------------------------

        private void HandleInput() {
            // horizontal
            m_Horizontal = Input.GetAxis("Horizontal");
            // jump
            var inputJump = Input.GetButtonDown("Jump");
            if (inputJump && !m_JumpPending) {
                m_JumpPending = true;
            }
            // fire
            var inputAttack = Input.GetButtonDown("Fire1");
            if (inputAttack && !m_AttackPending) {
                m_AttackPending = true;
            }
        }

        //--------------------------------------------
        // Movement
        //--------------------------------------------

        private void Move() {

            if (IsAttacking()) return;

            // move the player
            m_MoveDirection = new Vector3(m_Horizontal, 0, 0);
            m_MoveDirection = m_MoveDirection * speed * Time.deltaTime;
            m_Rigidbody.MovePosition(transform.position + m_MoveDirection);

            // only toggle flag when there is input
            if (m_Horizontal > 0) {
                m_IsFacingRight = true;
            } else if (m_Horizontal < 0) {
                m_IsFacingRight = false;
            }

            // snap the rotation to left or right, but still lerp it
            Vector3 facingDirection;
            if (m_IsFacingRight) {
                facingDirection = new Vector3(0, 90, 0);
            } else {
                facingDirection = new Vector3(0, 270, 0);
            }
            
            m_RotationDirection = Quaternion.Lerp(transform.rotation, Quaternion.Euler(facingDirection), turnSpeed * Time.deltaTime);
            m_Rigidbody.MoveRotation(m_RotationDirection);

        }

        private void Jump() {

            // if attacking eat the input
            if (IsAttacking()) {
                m_JumpPending = false;
                return;
            }

            // jump grabbed from input
            if (m_JumpPending) {

                if (IsGrounded()) {
                    // first jump
                    m_UsedDoubleJump = false;
                    DoJump(jumpSpeed);
                } else if (canDoubleJump && !m_UsedDoubleJump) {
                    // double jump
                    m_UsedDoubleJump = true;
                    DoJump(doubleJumpSpeed);
                }
                
            }

            m_JumpPending = false;

        }

        private void DoJump(float jSpeed) {
            // clear rigidbody y velocity
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z);
            m_Rigidbody.angularVelocity = Vector3.zero;
            // add y force
            m_Rigidbody.AddForce(new Vector3(0, jSpeed, 0), ForceMode.Impulse);
            // play audio clip
            m_AudioSource.PlayOneShot(jumpClip);
        }
        
        // IsGrounded - used for jumping
        private bool IsGrounded() {
            // check to see if one of the Ground Check points hit the ground
            foreach (var groundCheck in groundChecks) {
                Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundCheckDistance)) {
                    return true;
                }
            }
            return false;
        }

        private void AnimateMovement() {

            // IsGrounded() is expensive because of Raycast. 
            // Will have to just check velocity to handle animation
            bool grounded = (Mathf.Abs(m_Rigidbody.velocity.y) <= deadZone);
            m_Animator.SetBool("IsGrounded", grounded);
            if (grounded) {
                m_Animator.SetBool("IsMoving", m_Horizontal != 0);
            } else {
                m_Animator.SetBool("IsMoving", false);
            }

            m_Animator.SetBool("IsJumping", m_Rigidbody.velocity.y > deadZone);
            m_Animator.SetBool("IsFalling", m_Rigidbody.velocity.y < deadZone);

        }

        //--------------------------------------------
        // Attack
        //--------------------------------------------
        private void Attack() {
            
            if (m_TimerAttack > 0) {
                m_TimerAttack -= Time.deltaTime;
                if (m_TimerAttack <= 0) {
                    // clear rigidbody velocity
                    m_Rigidbody.velocity = Vector3.zero;
                    m_Rigidbody.angularVelocity = Vector3.zero;
                    // stop sliding
                    m_Rigidbody.useGravity = true;
                }
            } else {
                m_TimerAttack = 0;
            }

            if (m_AttackPending && !IsAttacking()) {
                if (IsGrounded()) {
                    DoAttack();
                }
            }

            m_AttackPending = false;
        }

        private void DoAttack() {
            // set delay timer
            m_TimerAttack = attackDuration;
            // clear rigidbody velocity
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
            // move forward a little bit
            m_Rigidbody.AddForce(transform.forward * attackForce, ForceMode.Impulse);
            // animate
            m_Animator.SetTrigger("Attack");
            // slide
            m_Rigidbody.useGravity = false;
            // play adio
            m_AudioSource.PlayOneShot(punchClip);
        }

        // called by Animation Event
        public void AttackAnimEvent() {
            punchHitbox.DealDamageToTargets(m_CharacterStats.damage);
        }

    }

}