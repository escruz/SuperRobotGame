using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    public class Collectable : QuestObjective {

        private bool collected = false;

        public override bool IsComplete() {
            return collected;
        }

        private void OnTriggerEnter(Collider other) {
            // deactivate on player hit
            if (other.tag == "Player") {
                collected = true;
                gameObject.SetActive(false);
            }
        }

    }

}