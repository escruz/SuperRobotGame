using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // stores the stats for the character
    public class CharacterStats : MonoBehaviour {

        public int maxHp = 1;
        public int hp = 1;
        public int damage = 1;

        public GameObject deathEffect;

        public bool IsDead() {
            return hp == 0;
        }

        public void DealDamage(int dmg) {
            if (IsDead()) return;
            hp -= dmg;
            if (hp <= 0) {
                hp = 0;
                Death();
            }
        }

        private void Death() {
            // TODO object pooler instead of instantiate
            Instantiate(deathEffect);
            gameObject.SetActive(false);
        }
        
    }

}