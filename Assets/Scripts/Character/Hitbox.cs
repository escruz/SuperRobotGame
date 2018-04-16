using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // stores the targets that are inside the trigger
    public class Hitbox : MonoBehaviour {

        public bool isPlayer = false;
        public List<GameObject> targets = new List<GameObject>();

        private void OnTriggerEnter(Collider other) {
            // add the unit to the target list based on hitbox type
            if (isPlayer && other.tag == "Enemy"
                || !isPlayer && other.tag == "Player") {
                if (!targets.Contains(other.gameObject)) {
                    targets.Add(other.gameObject);
                }
            }
        }
        
        private void OnTriggerExit(Collider other) {
            // remove the unit from the target list
            if (other.tag == "Enemy" || other.tag == "Player") {
                if (targets.Contains(other.gameObject)) {
                    targets.Remove(other.gameObject);
                }
            }
        }

        public void DealDamageToTargets(int damage) {
            for (int i = targets.Count-1; i>=0; i--) {
                var target = targets[i];
                // if target is invalid just remove and continue
                if (target == null) {
                    targets.Remove(target);
                    continue;
                }
                // damage target
                var stats = target.GetComponent<CharacterStats>();
                stats.DealDamage(damage);
                // remove dead targets
                if (stats.IsDead()) {
                    targets.Remove(target);
                }
            }
        }

    } 

}
