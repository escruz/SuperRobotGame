using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // A very camera that follows the player
    public class CameraFollow : MonoBehaviour {

        public Transform target;
        public float smoothing = 10f;
        public Vector3 offset = new Vector3(0,3,-12);

        float deadZone = 0.5f;
        
        private void FixedUpdate() {
            Follow();            
        }
        
        void Follow() {
            
            Vector3 newOffset = target.position + offset;
            newOffset.y = target.position.y;
            Debug.Log(Vector3.Distance(transform.position, newOffset));
            // if distance is really close dont move or else it will stutter
            if (Vector3.Distance(transform.position, newOffset) > deadZone) {
                transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
            }
            
        }

    }

}