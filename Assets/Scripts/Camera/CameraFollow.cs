using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // A very camera that follows the player
    public class CameraFollow : MonoBehaviour {

        public Transform target;
        public float smoothing = 10f;
        public Vector3 offset = new Vector3(5,2,-12);
        
        private void FixedUpdate() {
            Follow();            
        }
        
        void Follow() {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
        }

    }

}