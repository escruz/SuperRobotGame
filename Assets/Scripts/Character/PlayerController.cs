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
        
        float deadZone = 0.01f;

        Rigidbody m_RigidBody;
        Animator m_Animator;

        Vector3 m_MoveDirection;
        Quaternion m_RotationDirection;

        float horizontal;
        bool jumpPending;
        bool attackPending;

        bool isFacingRight = true;
        float timerAttack = 0;
        bool usedDoubleJump = false;

        //--------------------------------------------
        // Unity Methods
        //--------------------------------------------

        private void Awake() {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
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
            return timerAttack != 0;
        }

        //--------------------------------------------
        // Controls
        //--------------------------------------------

        private void HandleInput() {
            horizontal = Input.GetAxis("Horizontal");
            var inputJump = Input.GetButtonDown("Jump");
            if (inputJump && !jumpPending) {
                jumpPending = true;
            }
            var inputAttack = Input.GetButtonDown("Fire1");
            if (inputAttack && !attackPending) {
                attackPending = true;
            }
        }

        //--------------------------------------------
        // Movement
        //--------------------------------------------

        private void Move() {

            if (IsAttacking()) return;

            // move the player
            m_MoveDirection = new Vector3(horizontal, 0, 0);
            m_MoveDirection = m_MoveDirection * speed * Time.deltaTime;
            m_RigidBody.MovePosition(transform.position + m_MoveDirection);

            // only toggle flag when there is input
            if (horizontal > 0) {
                isFacingRight = true;
            } else if (horizontal < 0) {
                isFacingRight = false;
            }

            Vector3 facingDirection;
            if (isFacingRight) {
                facingDirection = new Vector3(0, 90, 0);
            } else {
                facingDirection = new Vector3(0, 270, 0);
            }
            
            m_RotationDirection = Quaternion.Lerp(transform.rotation, Quaternion.Euler(facingDirection), turnSpeed * Time.deltaTime);
            m_RigidBody.MoveRotation(m_RotationDirection);

        }

        private void Jump() {

            if (IsAttacking()) {
                // if attacking eat the input
                jumpPending = false;
                return;
            }

            if (jumpPending) {

                if (IsGrounded()) {
                    usedDoubleJump = false;
                    DoJump(jumpSpeed);
                } else if (canDoubleJump && !usedDoubleJump) {
                    usedDoubleJump = true;
                    DoJump(doubleJumpSpeed);
                }
                
            }

            jumpPending = false;

        }

        private void DoJump(float jSpeed) {
            m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0, m_RigidBody.velocity.z);
            m_RigidBody.angularVelocity = Vector3.zero;
            m_RigidBody.AddForce(new Vector3(0, jSpeed, 0), ForceMode.Impulse);
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
            bool grounded = (Mathf.Abs(m_RigidBody.velocity.y) <= deadZone);
            m_Animator.SetBool("IsGrounded", grounded);
            if (grounded) {
                m_Animator.SetBool("IsMoving", horizontal != 0);
            } else {
                m_Animator.SetBool("IsMoving", false);
            }

            m_Animator.SetBool("IsJumping", m_RigidBody.velocity.y > deadZone);
            m_Animator.SetBool("IsFalling", m_RigidBody.velocity.y < deadZone);

        }

        //--------------------------------------------
        // Attack
        //--------------------------------------------
        private void Attack() {
            
            if (timerAttack > 0) {
                timerAttack -= Time.deltaTime;
                if (timerAttack <= 0) {
                    m_RigidBody.velocity = Vector3.zero;
                    m_RigidBody.angularVelocity = Vector3.zero;
                    m_RigidBody.useGravity = true;
                }
            } else {
                timerAttack = 0;
            }

            if (attackPending && !IsAttacking()) {
                if (IsGrounded()) {
                    DoAttack();
                }
            }

            attackPending = false;
        }

        private void DoAttack() {
            timerAttack = attackDuration;
            m_RigidBody.velocity = Vector3.zero;
            m_RigidBody.angularVelocity = Vector3.zero;
            m_RigidBody.AddForce(transform.forward * attackForce, ForceMode.Impulse);
            m_Animator.SetTrigger("Attack");
            m_RigidBody.useGravity = false;
        }

    }

}