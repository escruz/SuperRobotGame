using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // stores the stats for the character
    public class CharacterStats : MonoBehaviour {

        public int maxHp = 1;
        public int hp = 1;
        public int damage = 1;

        public float invulDuration = 0;

        public GameObject deathEffect;

        float m_TimerInvul = 0;

        private void Update() {
            RunInvulnerableTimer();
        }

        public bool IsDead() {
            return hp == 0;
        }

        public bool IsInvulnerable() {
            return m_TimerInvul > 0;
        }

        public void DealDamage(int dmg) {
            if (IsDead()) return;
            if (IsInvulnerable()) return;
            hp -= dmg;
            if (hp <= 0) {
                hp = 0;
                Death();
            } else {
                m_TimerInvul = invulDuration;
            }
        }

        private void RunInvulnerableTimer() {
            if (m_TimerInvul > 0) {
                m_TimerInvul -= Time.deltaTime;
            } else {
                m_TimerInvul = 0;
            }
        }

        private void Death() {
            // TODO object pooler for explosions instead of instantiate
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        
    }

}