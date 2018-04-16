using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Robo {

    // stores the stats for the character
    public class CharacterStats : MonoBehaviour, IDataResettable {

        public int maxHp = 1;
        public int hp = 1;
        public int damage = 1;

        public float invulDuration = 0;

        public GameObject deathEffect;

        public AudioClip hitClip;

        AudioSource m_AudioSource;
        float m_TimerInvul = 0;

        public event UnityAction OnUpdateHP;

        private void Awake() {
            m_AudioSource = GetComponent<AudioSource>();
        }

        private void Update() {
            RunInvulnerableTimer();
        }

        // IDataResettable
        public void DataReset() {
            FullHeal();
        }

        //--------------------------------------------
        // Flags
        //--------------------------------------------
        public bool IsDead() {
            return hp == 0;
        }

        public bool IsInvulnerable() {
            return m_TimerInvul > 0;
        }

        //--------------------------------------------
        // Calculation
        //--------------------------------------------
        public void DealDamage(int dmg) {
            if (IsDead()) return;
            if (IsInvulnerable()) return;
            hp -= dmg;
            if (hp <= 0) {
                // dead
                hp = 0;
                Death();
            } else {
                // trigger iframes
                m_TimerInvul = invulDuration;
                // play hit clip
                m_AudioSource.PlayOneShot(hitClip);
            }
            if (OnUpdateHP != null) {
                OnUpdateHP();
            }
        }

        public void FullHeal() {
            hp = maxHp;
            if (OnUpdateHP != null) {
                OnUpdateHP();
            }
        }

        private void RunInvulnerableTimer() {
            // run timer
            if (m_TimerInvul > 0) {
                m_TimerInvul -= Time.deltaTime;
            } else {
                m_TimerInvul = 0;
            }
        }

        private void Death() {
            // TODO object pooler for explosions instead of instantiate
            // create explosion
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            // deactivate
            gameObject.SetActive(false);
        }
        
    }

}