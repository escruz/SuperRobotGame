using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {
    
    public class TitleScreenUI : MonoBehaviour {

        public void StartGame() {
            GameController.instance.LoadMainGame();
        }

    } 

}
