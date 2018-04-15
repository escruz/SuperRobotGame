using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // heals the Player when entering this zone
    public class HealZone : MonoBehaviour {

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                other.GetComponent<CharacterStats>().FullHeal();
            }
        }

    }

}