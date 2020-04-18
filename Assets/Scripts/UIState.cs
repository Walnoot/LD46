using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : MonoBehaviour {
    public GameObject upgradeDialog;
    
    void Update() {
        Time.timeScale = upgradeDialog.activeSelf ? 0f : 1f;
    }
}
