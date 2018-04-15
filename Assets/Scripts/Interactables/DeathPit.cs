using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // pit the player can fall off
    public class DeathPit : MonoBehaviour {

        public Transform pitSpawn; // the spawnpoint to return the player if not dead
        public int damage = 1;

        float delaySpawn = 1;
        GameObject player;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                player = other.gameObject;
                DamagePlayer();
            }
        }

        private void DamagePlayer() {
            var stats = player.GetComponent<CharacterStats>();
            // damage the player
            stats.DealDamage(damage);

            if (!stats.IsDead()) {
                // delay spawn player
                Invoke("SpawnPlayer", delaySpawn);
            }
        }

        private void SpawnPlayer() {
            // place the player on a spawn point if player is not dead
            player.transform.position = pitSpawn.position;
        }

    } 

}
