using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthLabel : MonoBehaviour {
    private Text text;

    private int curTextNumber = -999;
    private RadioTower tower;
    void Start() {
        tower = FindObjectOfType<RadioTower>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (tower != null) {
            if (curTextNumber != tower.health) {
                curTextNumber = tower.health;
                text.text = "❤️ Helth: "+ curTextNumber.ToString();
            }
        }
    }
}
