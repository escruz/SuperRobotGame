using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // handles quest requirements
    public class LevelController : MonoBehaviour {

        public string questMessage;
        public float messageDuration = 3f;
        public float victoryDuration = 3f;

        // quest condition (would be better if dynamic, but these are hardcoded for now)
        public bool killAllEnemies = false;
        public bool collectAllCoins = false;
        public bool runToTheFinish = false;

        private float m_TimeVictory = 0;

        // states
        enum LevelControllerStates {
            QuestCheck,
            Finish
        }

        private LevelControllerStates state;

        GameObject[] m_Enemies;

        private void Start() {
            if (!string.IsNullOrEmpty( questMessage )) {
                NotificationUI.instance.Show(questMessage, messageDuration);
            }
            if (killAllEnemies) {
                m_Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            }
            state = LevelControllerStates.QuestCheck;
        }

        private void Update() {

            // running in a very simple statemachine
            if (state == LevelControllerStates.QuestCheck) {
                QuestCheckUpdate();
            } else if (state == LevelControllerStates.Finish) {
                FinishUpdate();
            }
            
        }

        private void QuestCheckUpdate() {
            bool success = true;

            if (killAllEnemies) {
                if (!KilledAllEnemies()) {
                    success = false;
                }
            }
            // TODO other quests

            if (success) {
                NotificationUI.instance.Show("Level Complete", victoryDuration);
                m_TimeVictory = victoryDuration;
                state = LevelControllerStates.Finish; // change state
            }
        }

        private bool KilledAllEnemies() {
            for (int i = 0; i<m_Enemies.Length; i++) {
                if (m_Enemies[i].activeSelf) {
                    return false;
                }
            }
            return true;
        }

        private void FinishUpdate() {
            m_TimeVictory -= Time.deltaTime;
            if (m_TimeVictory <= 0) {
                GameController.instance.Load("LevelSelect");
            }
        }

    } 

}
