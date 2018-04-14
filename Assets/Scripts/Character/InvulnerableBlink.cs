using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    public class InvulnerableBlink : MonoBehaviour {

        public Color invulnerableColor = Color.white;
        public float blinkInterval = 0.2f;

        CharacterStats m_CharacterStats;
        Renderer[] m_Renderers;
        Color[] m_RendererColors;
        bool isTinted = false;
        float m_TimerBlink = 0;

        private void Awake() {
            m_CharacterStats = GetComponent<CharacterStats>();
        }

        private void Start() {
            m_Renderers = GetComponentsInChildren<Renderer>();
            m_RendererColors = new Color[m_Renderers.Length];
            for (int i = 0; i<m_Renderers.Length; i++) {
                var rend = m_Renderers[i];
                m_RendererColors[i] = rend.material.GetColor("_Color");
            }
        }

        private void Update() {
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

            if (m_CharacterStats.IsInvulnerable()) {
                ToggleTint();
            } else if (isTinted) {
                ToggleTint();
            }
        }

        private void ToggleTint() {
            for (int i = 0; i < m_Renderers.Length; i++) {
                Color color;
                if (isTinted) {
                    color = m_RendererColors[i];
                } else {
                    color = invulnerableColor;
                }
                m_Renderers[i].material.SetColor("_Color", color);
            }
            isTinted = !isTinted;
        }

    } 

}
