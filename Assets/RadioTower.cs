using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTower : MonoBehaviour
{

	public int initialHealth = 100;
	public int health;
    private ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
    	particleSystem = this.GetComponentsInChildren<ParticleSystem>()[0];
    	health = initialHealth;
    }

    // Update is called once per frame
    void Update()
    {
         var deathAmount = 1.0f - ((float)health/(float)initialHealth);
         particleSystem.startLifetime = deathAmount * 4;
    }

    public void hit(int damage){
    	health -= damage;
    	if(health <= 0){
    		dead();
    	}
    }

    void dead(){
		health = 0;
	}
}
