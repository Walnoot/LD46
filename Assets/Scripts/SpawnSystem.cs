using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnSystem : MonoBehaviour
{

    public int wave = 0;

	Spawner[] spawners;

	private float MIN_TIME_BETWEEN_SPAWNS = 5.0f;
	private float spawnTimer = 0f;
	private int spawnsLeft;

	public GameObject[] mobTypes;

	private Mob[] mobs;
	
    // Start is called before the first frame update
    void Start()
    {
		this.spawners = GameObject.FindObjectsOfType<Spawner>();
		if(this.spawners.Length == 0 || mobTypes.Length == 0){
			throw new Exception("Failed to load SpawnSystem.");
		}
		
		mobs = GameObject.FindObjectsOfType<Mob>();
    }

    // Update is called once per frame
    void Update()
    {
	    bool done = true;

	    foreach (var mob in mobs) {
		    if (!(mob == null || !mob.enabled)) {
			    done = false;
		    }
	    }

	    if (done) {
		    spawnTimer -= Time.deltaTime;

		    if (spawnTimer < 0f) {
			    startNewWave();
		    }
	    } else {
		    spawnTimer = MIN_TIME_BETWEEN_SPAWNS;
	    }
    }

    void startNewWave(){
        wave ++;
        this.spawnsLeft = (int) Math.Pow(1.5, wave);
        Debug.Log("startNewWave"+wave);

        while (spawnsLeft > 0) {
	        spawnsLeft -= spawnSome(spawnsLeft);
        }
        
        mobs = GameObject.FindObjectsOfType<Mob>();
    }

    private int spawnSome(int max) {
	    var spawner = spawners[UnityEngine.Random.Range(0, spawners.Length)];
	    var mobType = mobTypes[UnityEngine.Random.Range(0, mobTypes.Length)];
	    if(wave < 2){
		    mobType = mobTypes[0];
	    }
	    int rows = UnityEngine.Random.Range(1,3);
	    int columns = UnityEngine.Random.Range(1,3);
	    return spawner.spawnRectangle(rows, columns, max, mobType);
    }

    public float getSpawnsLeft() {
    	return this.spawnsLeft;
    }
}
