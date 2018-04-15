using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // handles character kill quest objective
    public class KillQuestObjective : QuestObjective {

        CharacterStats m_Stats;

        private void Awake() {
            m_Stats = GetComponent<CharacterStats>();
        }

        // quest complete if character is dead
        public override bool IsComplete() {
            return m_Stats.IsDead();
        }

    }

}