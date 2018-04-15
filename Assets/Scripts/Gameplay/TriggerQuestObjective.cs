using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    public class TriggerQuestObjective : QuestObjective {

        private bool triggered = false;

        public override bool IsComplete() {
            return triggered;
        }

        private void OnTriggerEnter(Collider other) {
            // deactivate on player hit
            if (other.tag == "Player") {
                triggered = true;
            }
        }

    }

}