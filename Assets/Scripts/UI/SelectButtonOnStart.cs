using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// selects the button by default on enable
public class SelectButtonOnStart : MonoBehaviour {

    void OnEnable() {
        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected != gameObject) {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
            var button = GetComponent<Button>();
            button.Select();
        }
    }

}
