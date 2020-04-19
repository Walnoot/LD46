using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveLabel : MonoBehaviour {
    private Text text;

    // private int curText = -999;
    private SpawnSystem spawnSystem;
    void Start() {
        spawnSystem = FindObjectOfType<SpawnSystem>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (spawnSystem != null) {
            var wave = spawnSystem.wave;
            var spawnsLeft = spawnSystem.getSpawnsLeft();
            var newText = " ~ Wave: " + wave 
                      + "\n < Spawns Left: " + spawnsLeft;
            text.text = newText;
        }
    }
}
