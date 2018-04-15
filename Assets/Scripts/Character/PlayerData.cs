using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // stores progress for the player
    public class PlayerData : MonoBehaviour {

        // stores the level names completed
        public List<string> completedLevels = new List<string>();

        public void CompleteLevel(string levelName) {
            completedLevels.Add(levelName);
        }

        public bool IsLevelCompleted(string levelName) {
            return completedLevels.Contains(levelName);
        }

    }

}