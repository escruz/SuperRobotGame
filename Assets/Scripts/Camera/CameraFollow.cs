using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // A very camera that follows the player
    public class CameraFollow : MonoBehaviour {

        public GameObject target;
        public float smoothing = 10f;
        public Vector3 offset = new Vector3(0,3,-12);

        float deadZone = 0.1f;
        
        private void FixedUpdate() {
            AssignTargetIfNeeded();
            Follow();            
        }

        // assign the player if target is null
        void AssignTargetIfNeeded() {
            if (target != null) return;
            target = GameObject.FindGameObjectWithTag("Player");
        }
        
        // follow the target gameobject
        void Follow() {

            if (target == null) return;

            Vector3 newOffset = target.transform.position + offset;

            // if distance is really close dont move or else it will stutter
            if (Vector3.Distance(transform.position, newOffset) > deadZone) {
                transform.position = Vector3.Lerp(transform.position, newOffset, smoothing * Time.deltaTime);
            }
            
        }

    }

}