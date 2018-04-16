using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // Tints the shader color when character is invulnerable
    public class InvulnerableBlink : MonoBehaviour {
        
        // tinted color
        public Color invulnerableColor = Color.black;
        public float blinkInterval = 0.2f;

        CharacterStats m_CharacterStats;
        Renderer[] m_Renderers;
        bool isTinted = false;
        float m_TimerBlink = 0;

        private void Awake() {
            m_CharacterStats = GetComponent<CharacterStats>();
        }

        private void Start() {
            m_Renderers = GetComponentsInChildren<Renderer>();
        }

        private void Update() {
            // if dead toggle to normal
            if (m_CharacterStats.IsDead()) {
                if (isTinted) {
                    ToggleTint();
                }
                return;
            }
            Blink();
        }

        void Blink() {

            // only execute per interval
            if (m_TimerBlink > 0) {
                m_TimerBlink -= Time.deltaTime;
                return;
            } else {
                m_TimerBlink = blinkInterval;
            }

            // toggle tint when invulnerable
            // if not toggle to change back to regular tint
            if (m_CharacterStats.IsInvulnerable()) {
                ToggleTint();
            } else if (isTinted) {
                ToggleTint();
            }

        }

        private void ToggleTint() {
            // loop thru renderers
            for (int i = 0; i < m_Renderers.Length; i++) {
                Color color;
                if (isTinted) {
                    color = Color.white;
                } else {
                    color = invulnerableColor;
                }
                // loop thru materials per renderer
                for (var j = 0; j < m_Renderers[i].materials.Length; j++) {
                    m_Renderers[i].materials[j].SetColor("_Tint", color);
                }
            }
            isTinted = !isTinted; // toggle
        }

    } 

}
