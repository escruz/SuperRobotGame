using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {
    
    public class EnemyChase : MonoBehaviour {

        [Header("Move Speed")]
        public float speed = 3f;
        public float speedSmoothing = 10f;
        public float turnSpeed = 10f;

        public Hitbox enemyDetection;

        float deadZone = 0.01f;

        CharacterStats m_CharacterStats;
        Rigidbody m_Rigidbody;
        Animator m_Animator;
        Vector3 m_MoveDirection;
        Quaternion m_RotationDirection;

        bool m_IsFacingRight = false;
        float m_Horizontal;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
            m_CharacterStats = GetComponent<CharacterStats>();
        }

        private void FixedUpdate() {
            ChasePlayer();
            AnimateMovement();
        }

        private void OnCollisionStay(Collision collision) {
            if (collision.gameObject.tag == "Player") {
                var playerStats = collision.gameObject.GetComponent<CharacterStats>();
                playerStats.DealDamage(m_CharacterStats.damage);
            }
        }

        // TODO this code is mostly a copy of PlayerController Move()
        // Need to refactor this somehow in the future
        private void ChasePlayer() {

            // if cant find player just stand still
            if (enemyDetection.targets.Count == 0) {
                m_Horizontal = 0;
                m_Rigidbody.velocity = Vector3.zero;
                m_Rigidbody.angularVelocity = Vector3.zero;
                return;
            }

            // get the player
            var target = enemyDetection.targets[0];

            // move the enemy towards the player
            m_Horizontal = target.transform.position.x < transform.position.x ? -1 : 1;
            m_MoveDirection = new Vector3(m_Horizontal, 0, 0);
            m_MoveDirection = m_MoveDirection * speed * Time.deltaTime;
            m_Rigidbody.MovePosition(transform.position + m_MoveDirection);

            if (m_Horizontal > 0) {
                m_IsFacingRight = true;
            } else if (m_Horizontal < 0) {
                m_IsFacingRight = false;
            }

            Vector3 facingDirection;
            if (m_IsFacingRight) {
                facingDirection = new Vector3(0, 90, 0);
            } else {
                facingDirection = new Vector3(0, 270, 0);
            }

            m_RotationDirection = Quaternion.Lerp(transform.rotation, Quaternion.Euler(facingDirection), turnSpeed * Time.deltaTime);
            m_Rigidbody.MoveRotation(m_RotationDirection);

        }

        // TODO this code is mostly a copy of PlayerController AnimateMovement()
        // Need to refactor this somehow in the future
        private void AnimateMovement() {
            
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

    } 

}
