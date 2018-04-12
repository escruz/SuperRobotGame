using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // A very simple camera follow. Doesn't offer a great ton of smoothing since its run on FixedUpdate.
    // Will fix this to Update following a dummy transform when I have more time left.
    public class CameraFollow : MonoBehaviour {

        public Transform target;
        public float smoothing = 10f;
        public Vector3 offset;
        
        private void FixedUpdate() {
            
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
            
        }

    }

}