﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Robo {

    // singleton class that has reference to persisters
    // and controls scene changes
    public class GameController : MonoBehaviour {

        public static GameController instance {
            get {
                return m_Instance;
            }
        }

        private static GameController m_Instance;

        public GameObject player;

        public string titleScene = "TitleScreen";
        public string startGameScene = "LevelSelect";

        private string currentScene = "";
        
        public event UnityAction OnFinishedLoading;

        private void Awake() {
            if (m_Instance == null) {
                m_Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            LoadTitleScene();
        }

        public void LoadTitleScene() {
            Load(titleScene);
        }

        public void LoadMainGame() {
            Load(startGameScene);
        }

        //--------------------------------------------
        // Scene Management
        //--------------------------------------------
        public void Load(string sceneName) {
            if (currentScene == sceneName) return;
            Unload(currentScene);
            currentScene = sceneName;
            StartCoroutine(LoadAdditiveAsync(sceneName));
        }

        public void Unload(string sceneName) {
            if (string.IsNullOrEmpty(sceneName)) return;
            SceneManager.UnloadSceneAsync(sceneName);
        }

        private IEnumerator LoadAdditiveAsync(string sceneName) {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncOperation.priority = 0;
            yield return asyncOperation;
            Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newlyLoadedScene);
            Spawn();
        }

        //--------------------------------------------
        // Player Spawn
        //--------------------------------------------
        public void Spawn() {

            var spawnpoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            player.transform.position = spawnpoint.transform.position;
            
        }

    }

}