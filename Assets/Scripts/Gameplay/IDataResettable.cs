using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robo {

    // interface that gets called when game is reset (Title Scene)
    public interface IDataResettable {
        void DataReset();
    }

}
