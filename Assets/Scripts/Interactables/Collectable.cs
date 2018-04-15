using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    public class Collectable : MonoBehaviour {

        private void OnTriggerEnter(Collider other) {
            // deactivate on player hit
            if (other.tag == "Player") {
                gameObject.SetActive(false);
            }
        }

    }

}