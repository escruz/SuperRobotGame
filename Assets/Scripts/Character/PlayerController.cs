using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    public class PlayerController : MonoBehaviour {
        
        public float speedSmoothing = 10f;
        public float speed = 6f;
        public float jumpSpeed = 4f;
        public float groundCheckDistance = 0.5f;

        public Transform[] groundChecks;

        Rigidbody m_RigidBody;
        Vector3 m_MoveDirection;

        float horizontal;
        bool inputJump;
        bool jumpPending;

        private void Awake() {
            m_RigidBody = GetComponent<Rigidbody>();
        }
        
        private void Update() {
            horizontal = Input.GetAxis("Horizontal");
            inputJump = Input.GetButtonDown("Jump");
            if (inputJump && !jumpPending) {
                jumpPending = true;
            } 
        }

        private void FixedUpdate() {
            Move();
            Jump();
        }

        private void Move() {
            m_MoveDirection = new Vector3(horizontal, 0, 0);
            m_MoveDirection = m_MoveDirection * speed * Time.deltaTime;
            m_RigidBody.MovePosition(transform.position + m_MoveDirection);
        }

        private void Jump() {
            if (jumpPending && IsGrounded()) {
                //m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, m_RigidBody.velocity.y * jumpSpeed, m_RigidBody.velocity.z);
                m_RigidBody.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
            }
            jumpPending = false;
        }

        private bool IsGrounded() {
            foreach (var groundCheck in groundChecks) {
                Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundCheckDistance)) {
                    return true;
                }
            }
            return false;

        }

    }

}