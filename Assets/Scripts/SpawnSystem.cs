using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnSystem : MonoBehaviour
{

    public int wave = 0;
	public bool running = false;

	Spawner[] spawners;

	private float MIN_TIME_BETWEEN_SPAWNS = 5.0f;
	private float timeout = 0f;
	private int spawnsLeft = 0;

	public GameObject[] mobTypes;

    // Start is called before the first frame update
    void Start()
    {
		this.spawners = GameObject.FindObjectsOfType<Spawner>();
		if(this.spawners.Length == 0 || mobTypes.Length == 0){
			throw new Exception("Failed to load SpawnSystem.");
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            startNewWave();
        }
        if(!running) {
        	return;
        }
        if(spawnsLeft <= 0) {
			var mobs = GameObject.FindObjectsOfType<Mob>();
			// TODO: Check if any mob is alive instead
			if(mobs.Length == 0){
				onWaveCompleted();
				return;
			} else {
				// wait for wave to clear
			}
        } else {
        	timeout -= Time.deltaTime;
        	if(timeout <= 0){
        		timeout = MIN_TIME_BETWEEN_SPAWNS;
        		spawnSome();
        	}
        }
    }

    void spawnSome() {
    	var spawner = spawners[UnityEngine.Random.Range(0, spawners.Length)];
        var mobType = mobTypes[UnityEngine.Random.Range(0, mobTypes.Length)];
        if(wave < 2){
            mobType = mobTypes[0];
        }
    	int rows = UnityEngine.Random.Range(1,3);
    	int columns = UnityEngine.Random.Range(1,3);
    	int max = spawnsLeft;
    	spawner.spawnRectangle(rows, columns, max, mobType);
        spawnsLeft -= Math.Min(rows * columns, max);
    }

    void startNewWave(){
        if(running){
            return;
        }
        wave ++;
        spawnsLeft = (int) Math.Pow(1.5, wave);
        running = true;        
        Debug.Log("startNewWave"+wave);
    }

    void onWaveCompleted(){
        running = false;
    }
}
