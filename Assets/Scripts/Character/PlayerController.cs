using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // Controling movement and attacks for the player
    public class PlayerController : MonoBehaviour {
        
        [Header("Speed")]
        public float speed = 6f;
        public float speedSmoothing = 10f;
        public float turnSpeed = 10f;
        public float jumpSpeed = 8f;

        [Header("Ground Checks")]
        [Tooltip("Points to use for raycasting to the ground")]
        public Transform[] groundChecks;
        public float groundCheckDistance = 0.5f;

        float deadZone = 0.01f;

        Rigidbody m_RigidBody;
        Animator m_Animator;
        Vector3 m_MoveDirection;
        Quaternion m_RotationDirection;

        float horizontal;
        bool jumpPending;
        bool attackPending;

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
            Animate();
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

        private void Move() {
            // move the player
            m_MoveDirection = new Vector3(horizontal, 0, 0);
            m_MoveDirection = m_MoveDirection * speed * Time.deltaTime;
            m_RigidBody.MovePosition(transform.position + m_MoveDirection);

            // rotate the player to the movement direction
            if (m_MoveDirection != Vector3.zero) {
                m_RotationDirection = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_MoveDirection), turnSpeed * Time.deltaTime);
                m_RigidBody.MoveRotation(m_RotationDirection);
            }
        }

        private void Jump() {
            if (jumpPending && IsGrounded()) {
                m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0, m_RigidBody.velocity.z);
                m_RigidBody.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
            }
            jumpPending = false;
        }
        
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

        private void Attack() {
            // TODO
            if (attackPending) {
                m_Animator.SetTrigger("Attack");
            }
            attackPending = false;
        }

        private void Animate() {
            // IsGrounded() is expensive because of Raycast. 
            // Will have to just check velocity to handle animation
            var onGround = (Mathf.Abs(m_RigidBody.velocity.y) <= deadZone);

            m_Animator.SetBool("IsGrounded", onGround);
            if (onGround) {
                m_Animator.SetBool("IsMoving", horizontal != 0);
            } else {
                m_Animator.SetBool("IsMoving", false);
            }

            m_Animator.SetBool("IsJumping", m_RigidBody.velocity.y > deadZone);
            m_Animator.SetBool("IsFalling", m_RigidBody.velocity.y < deadZone);
        }

    }

}