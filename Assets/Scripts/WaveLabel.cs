using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveLabel : MonoBehaviour {
    private Text text;
	public bool ars;
	public bool beep;

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
            var left = spawnSystem.getCountMobsAlive();
            var total = spawnSystem.getCountMobsTotal();
			var newText = wave.ToString();
			if (ars)
				newText = left + " / " +total;
			if (beep)
				newText = "You kept your 5G tower alive for " + wave + " days!";
			text.text = newText;
        }
    }
}
