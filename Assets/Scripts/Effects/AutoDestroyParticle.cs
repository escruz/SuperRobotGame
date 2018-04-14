using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// auto destroy/deactivate a partile effect after it finishes
public class AutoDestroyParticle : MonoBehaviour {

    public bool deactivate;
    private ParticleSystem particle;

    private void Start() {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update() {
        if (!particle.IsAlive()) {
            if (deactivate) {
                gameObject.SetActive(false);
            } else {
                Destroy(gameObject);
            }
        }
    }

}
