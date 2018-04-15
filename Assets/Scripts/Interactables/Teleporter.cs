using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // interactable that changes scene
    public class Teleporter : MonoBehaviour {

        public string sceneName;

        bool isPlayerInTrigger = false;

        public void Update() {
            
            if (string.IsNullOrEmpty(sceneName)) return;

            // input dpad up
            if (Input.GetAxisRaw("Vertical") == 1) {
                GameController.instance.Load(sceneName);
            }

        }

        private void OnTriggerEnter(Collider other) {
            // player in range
            if (other.tag == "Player") {
                isPlayerInTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            // player out of range
            if (other.tag == "Player") {
                isPlayerInTrigger = false;
            }
        }

    } 

}
