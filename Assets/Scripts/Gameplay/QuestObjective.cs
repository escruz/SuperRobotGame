using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // base class for quest objectives
    public class QuestObjective : MonoBehaviour {
        public virtual bool IsComplete() {
            return true;
        }
    }

}
