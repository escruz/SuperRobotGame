using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // Shows the number of hearts based on player hp
    // currently shows only a limited amount of hearts
    public class PlayerHealthUI : MonoBehaviour {
        
        public List<GameObject> m_FilledHearts = new List<GameObject>();
        public List<GameObject> m_EmptyHearts = new List<GameObject>();

        CharacterStats m_Stats;

        private void Update() {
            AssignTargetIfNeeded();
        }

        // assign the player stats if target is null
        void AssignTargetIfNeeded() {
            if (m_Stats != null) return;
            var target = GameObject.FindGameObjectWithTag("Player");
            if (target != null) {
                m_Stats = target.GetComponent<CharacterStats>();
                RefreshHearts();
                m_Stats.OnUpdateHP += M_PlayerCharacterStats_OnUpdateHP;
            }
        }

        private void M_PlayerCharacterStats_OnUpdateHP() {
            RefreshHearts();
        }

        private void RefreshHearts() {
            for (int i = 0; i<m_Stats.maxHp; i++) {
                if (m_Stats.hp > i) {
                    m_FilledHearts[i].SetActive(true);
                    m_EmptyHearts[i].SetActive(false);
                } else {
                    m_FilledHearts[i].SetActive(false);
                    m_EmptyHearts[i].SetActive(true);
                }
            }
        }

    }

}