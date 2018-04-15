using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Robo {

    // interactable that changes scene
    public class Teleporter : MonoBehaviour {

        public string displayName;
        public string sceneName;
        public Text levelText;
        public Text completedText;
        public Text promptText;

        bool isPlayerInTrigger = false;

        private void Awake() {
            promptText.gameObject.SetActive(false);
        }

        private void OnEnable() {

            SetTextValues();
            
        }

        public void Update() {

            HandleInput();

        }

        private void OnTriggerEnter(Collider other) {
            // player in range
            if (other.tag == "Player") {
                isPlayerInTrigger = true;
                promptText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other) {
            // player out of range
            if (other.tag == "Player") {
                isPlayerInTrigger = false;
                promptText.gameObject.SetActive(false);
            }
        }

        private void HandleInput() {
            if (string.IsNullOrEmpty(sceneName)) return;
            if (!isPlayerInTrigger) return;
            // input dpad up
            if (Input.GetAxisRaw("Vertical") == 1) {
                GameController.instance.Load(sceneName);
            }
        }

        private void SetTextValues() {

            // level name to transition to
            levelText.text = displayName;

            // completed text if player finished the level
            completedText.gameObject.SetActive(false);
            var target = GameObject.FindGameObjectWithTag("Player");
            if (target != null) {
                var playerData = target.GetComponent<PlayerData>();
                if (playerData.IsLevelCompleted(sceneName)) {
                    completedText.gameObject.SetActive(true);
                }
            }
        }

    } 

}
