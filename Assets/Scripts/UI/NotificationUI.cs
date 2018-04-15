using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Robo {

    // singleton that displays a large text on the screen
    public class NotificationUI : MonoBehaviour {

        public static NotificationUI instance {
            get {
                return m_Instance;
            }
        }

        private static NotificationUI m_Instance;

        public Text notificationText;
        float m_TimerShow;

        private void Awake() {
            if (m_Instance == null) {
                m_Instance = this;
            } else {
                Destroy(gameObject);
            }
            notificationText.gameObject.SetActive(false);
        }

        private void Update() {

            // disable the text message after the duration is complete
            if (m_TimerShow > 0) {
                m_TimerShow -= Time.deltaTime;
                return;
            }
            notificationText.gameObject.SetActive(false);

        }

        // show a message to the screen
        public void Show( string message, float duration ) {
            m_TimerShow = duration;
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);
        }

    }

}