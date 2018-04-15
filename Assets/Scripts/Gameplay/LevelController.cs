using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Robo {

    // handles quest requirements
    public class LevelController : MonoBehaviour {
        
        public string questMessage;
        public float messageDuration = 3f;
        public float transitionDelay = 3f;

        // quest condition (would be better if dynamic scriptable objects, but these are hardcoded for now)
        public bool killAllEnemies = false;
        public bool collectAllCoins = false;
        public bool runToTheFinish = false;

        private float m_Delay = 0;
        private CharacterStats playerStats;

        // states
        enum LevelControllerStates {
            QuestCheck,
            Victory,
            Defeat
        }

        private LevelControllerStates state;

        GameObject[] m_Enemies;

        private void Start() {
            // show notification
            if (!string.IsNullOrEmpty( questMessage )) {
                NotificationUI.instance.Show(questMessage, messageDuration);
            }
            // grab required data for quest
            if (killAllEnemies) {
                m_Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            }
            // initialize state
            state = LevelControllerStates.QuestCheck;
        }

        // running in a very simple statemachine
        private void Update() {
            
            if (state == LevelControllerStates.QuestCheck) {
                QuestCheckUpdate();
            } else if (state == LevelControllerStates.Victory) {
                VictoryUpdate();
            } else if (state == LevelControllerStates.Defeat) {
                DefeatUpdate();
            }
            
        }

        private void QuestCheckUpdate() {

            // check player death for Defeat condition
            AssignTargetIfNeeded();
            if (playerStats != null) {
                if (playerStats.IsDead()) {
                    NotificationUI.instance.Show("Game Over", transitionDelay);
                    m_Delay = transitionDelay;
                    state = LevelControllerStates.Defeat; // change state
                    return;
                }
            }

            // check quest complete conditions
            bool success = true;

            if (killAllEnemies) {
                if (!KilledAllEnemies()) {
                    success = false;
                }
            }
            // TODO other quests

            if (success) {
                // register completed level on PlayerData
                playerStats.GetComponent<PlayerData>().CompleteLevel(SceneManager.GetActiveScene().name);
                // show notification
                NotificationUI.instance.Show("Level Complete", transitionDelay);
                // transition to different state
                m_Delay = transitionDelay;
                state = LevelControllerStates.Victory;
                return;
            }
        }

        private bool KilledAllEnemies() {
            // check if all enemies are deactivated
            for (int i = 0; i<m_Enemies.Length; i++) {
                if (m_Enemies[i].activeSelf) {
                    return false;
                }
            }
            return true;
        }

        private void VictoryUpdate() {
            m_Delay -= Time.deltaTime;
            if (m_Delay <= 0) {
                // go back to level select
                GameController.instance.Load("LevelSelect");
            }
        }

        private void DefeatUpdate() {
            m_Delay -= Time.deltaTime;
            if (m_Delay <= 0) {
                // quit to title
                GameController.instance.LoadTitleScene();
            }
        }

        // assign the player
        void AssignTargetIfNeeded() {
            if (playerStats != null) return;
            var target = GameObject.FindGameObjectWithTag("Player");
            if (target != null) {
                playerStats = target.GetComponent<CharacterStats>();
            }
        }

    } 

}
