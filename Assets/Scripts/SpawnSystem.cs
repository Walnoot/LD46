using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnSystem : MonoBehaviour
{

    public int wave = 0;

	Spawner[] spawners;

	private float MIN_TIME_BETWEEN_WAVE = 3.0f;
	private float timerBetweenWaves = 0f;

	public GameObject mobBasic = null; 
	public GameObject mobFarWanderer = null; 
	public GameObject mobDodge = null;
	public GameObject mobFastWanderer = null;

    // Start is called before the first frame update
    void Start()
    {
		this.spawners = GameObject.FindObjectsOfType<Spawner>();
		if(this.spawners.Length == 0 
			||	mobBasic == null
			||	mobFarWanderer == null
			||	mobDodge == null
			||  mobFastWanderer == null ) {
			throw new Exception("Failed to load SpawnSystem.");
		}
    }

    // Update is called once per frame
    void Update()
    {
    	if (Input.GetKeyDown(KeyCode.F5))
        {
            killAllMobs();
        }
	    bool done = getCountMobsAlive() == 0;

	    if (done) {
		    timerBetweenWaves -= Time.deltaTime;

		    if (timerBetweenWaves < 0f) {
			    startNewWave();
		    }
	    } else {
		    timerBetweenWaves = MIN_TIME_BETWEEN_WAVE;
	    }
    }

    void killAllMobs() {
        var mobs = GameObject.FindObjectsOfType<Mob>();
    	foreach(var mob in mobs) {
        	mob.die();
        	Destroy(mob);
        }
    }

    void startNewWave(){
        wave ++;
        int spawnsLeft = 0;
        var mobs = GameObject.FindObjectsOfType<Mob>();
        killAllMobs();
        switch(wave) {
    		case (0) :  { spawnsLeft = 0; break; }
    		case (1)  : { spawnsLeft = 10; break; }
    		case (2)  : { spawnsLeft = 20; break; }
    		case (3)  : { spawnsLeft = 30; break; }
    		case (4)  : { spawnsLeft = 40; break; }
    		case (5)  : { spawnsLeft = 50; break; }
    		case (6)  : { spawnsLeft = 60; break; }
    		case (7)  : { spawnsLeft = 70; break; }
    		case (8)  : { spawnsLeft = 80; break; }
    		case (9)  : { spawnsLeft = 90; break; }
    		case (10) : { spawnsLeft = 100; break; }
    		default : {	spawnsLeft = (int) Math.Pow(1.6, wave); break;};
    	}

    	while(spawnsLeft > 0) {
	    	spawnsLeft -= spawnSome(spawnsLeft);
		}
    }

    private int spawnSome(int max) {
    	GameObject[] types;
    	switch(wave) {
    		case (1)  : {types = new GameObject[] {mobBasic, mobFarWanderer}; break;}
    		case (2)  : {types = new GameObject[] {mobBasic, mobFarWanderer, mobDodge }; break;}
    		case (3)  : {types = new GameObject[] {mobBasic, mobFarWanderer, mobDodge }; break;}
    		case (4)  : {types = new GameObject[] {mobFarWanderer }; break;}
    		case (5)  : {types = new GameObject[] {mobBasic, mobFarWanderer, mobDodge, mobFastWanderer}; break;}
    		case (6)  : {types = new GameObject[] {mobBasic, mobFarWanderer, mobDodge, mobFastWanderer}; break;}
    		case (7)  : {types = new GameObject[] {mobBasic, mobFarWanderer, mobDodge, mobFastWanderer}; break;}
    		case (8)  : {types = new GameObject[] {mobFastWanderer}; break;}
    		case (9)  : {types = new GameObject[] {mobDodge}; break;}
    		case (10) : {types = new GameObject[] {mobBasic, mobFarWanderer, mobDodge, mobFastWanderer}; break;}
    		default : {types = new GameObject[] {mobBasic, mobFarWanderer, mobDodge, mobFastWanderer}; break;}
    	}
	    var spawner = spawners[UnityEngine.Random.Range(0, spawners.Length)];
	    var mobType = types[UnityEngine.Random.Range(0, types.Length)];
	    int rows = UnityEngine.Random.Range(2,6);
	    int columns = UnityEngine.Random.Range(2,6);
	    return spawner.spawnRectangle(rows, columns, max, mobType);
    }

    public int getCountMobsAlive() {
        var mobs = GameObject.FindObjectsOfType<Mob>();
    	if(mobs == null) {
    		return 0;
    	}
    	int count = 0;
    	foreach (var mob in mobs) {
		    if (mob != null && mob.enabled) {
			    count++;
		    }
	    }
	    return count;
    }

    public int getCountMobsTotal() {
        var mobs = GameObject.FindObjectsOfType<Mob>();
    	return mobs.Length;
    }
}
