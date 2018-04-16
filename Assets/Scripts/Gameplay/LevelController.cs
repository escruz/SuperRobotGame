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

        public QuestObjective[] questObjectives;

        private float m_Delay = 0;
        private CharacterStats playerStats;

        // states
        enum LevelControllerStates {
            QuestCheck,
            Victory,
            Defeat
        }

        private LevelControllerStates state;

        private void Start() {
            // show notification
            if (!string.IsNullOrEmpty( questMessage )) {
                NotificationUI.instance.Show(questMessage, messageDuration);
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

        // TODO move this to listeners instead of running on update
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
            if (CompletedAllQuests()) {
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

        private bool CompletedAllQuests() {
            // check if all IQuestObjectives are completed
            for (int i = 0; i < questObjectives.Length; i++) {
                if (!questObjectives[i].IsComplete()) {
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
