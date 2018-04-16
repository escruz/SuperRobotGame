using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    public class Collectable : QuestObjective {

        public AudioClip collectClip;
        private bool collected = false;

        public override bool IsComplete() {
            return collected;
        }

        private void OnTriggerEnter(Collider other) {
            // deactivate on player hit
            if (!collected && other.tag == "Player") {
                collected = true;
                // play audioclip on player's audiosource
                other.GetComponent<AudioSource>().PlayOneShot(collectClip); 
                gameObject.SetActive(false);
            }
        }

    }

}