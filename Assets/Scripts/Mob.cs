using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mob : MonoBehaviour
{

	enum State {
		Idle,
		Dodge,
		MoveToTower,
		Wandering,
		Igniting,
		Dead
	} 
	State state;

	// ATTACK 
	public float attackTimeout = 0.7f;
	public int attackDamage = 1;
	public float distanceAttack = 3f;
	private float attackTimeoutRemaining = 0f;

	// DODGE
	public bool hasCapabilityDodge = true;
	public float dodgeTriggerArea = 4f; //Distance to player
	public float dodgeSpeedMultiplier = 2f; 
	public float dodgeRotationSpeedMultiplier = 3f; 
	public float dodgeTime = 2f; // How long dodge lasts
	public float dodgeTimeout = 4f; // Desired time between dodges 
	private float dodgeTimeRemaining = 0f; 
	private float dodgeTimeoutRemaining = 2f;

	// WANDER
	public bool hasCapabilityWander = true;
	public float wanderMaxDuration = 5.0f; // max duration to reach a wanderGoal
	public int wanderingsLeft = 4; // how many times to pick a new wanderGoal
	public float wanderRange = 8; // how far to pick new wanderGoal
	/*private*/public float wanderTimeleft = 0f;
	/*private*/public Vector3 wanderGoal;

	public GameObject deathEffect;

	public GameObject PointPrefab, killSoundPrefab;
	public Animator animator;

    public float speed = 100.0f;
    public float rotationSpeed = 1f;

	private Rigidbody body;
    private GameObject tower;
    private GameObject dodgeObject;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        state = State.Idle;
		var towers = GameObject.FindGameObjectsWithTag("MobTarget");
		if(towers.Length > 0) {
			this.tower = towers[0];
		}
    }

    // Update is called once per frame
    void Update()
    {
    	if(isOutOfBounds()){
    		Debug.Log("Mob IsOutOfBounds." + body.position);
    		die();
    	}
    	switch(state) {
    		case(State.Idle):{
    			if(hasCapabilityWander && wanderingsLeft > 0 && wanderRange > 1) {
					wanderingsLeft --;
					var offset = new Vector3 (Random.Range(-wanderRange, wanderRange), 0, Random.Range(-wanderRange, wanderRange));
					wanderGoal = body.position + offset;
					// TODO: clamp to map bounds
					wanderTimeleft = wanderMaxDuration;
					state = State.Wandering;
					break;
    			} else {
    				state = State.MoveToTower;
    			}
    			break;
			} case(State.Wandering) : {
				if(tryDodgeTransition()){
					break;
				}
				if(wanderGoal != null) {
					wanderTimeleft -= Time.deltaTime;
					if(wanderTimeleft <= 0){
						state = State.Idle;
						break;
					}
					float dst = Vector3.Distance(wanderGoal, body.position);
					if ( dst <= 2f) {
						state = State.Idle;
						break;
					}
					walkTowards(wanderGoal);
				}
				break;
			} case(State.MoveToTower):{
    			if(tryDodgeTransition()){
					break;
				}
				this.animator.SetBool("Dodging", false);
    			if(tower != null) {
					float dst = Vector3.Distance(tower.transform.position, body.position);
					if(dst <= distanceAttack) {
						state = State.Igniting;
						break;
					}
    				walkTowards(tower.transform.position);
    			}
    			break;
    		} case(State.Dodge) : {
    			if(dodgeTimeRemaining > 0){
    				dodgeTimeRemaining -= Time.deltaTime;
    				Vector3 targetDir = dodgeObject.transform.position - body.position;
    				// Flip 
    				targetDir.x = -targetDir.x;
    				targetDir.z = -targetDir.z;
    				targetDir.y = 0;
                    targetDir.Normalize();

                    Vector3 mapEdge = transform.position;
                    mapEdge.y = 0f;
                    mapEdge.Normalize();

                    // blend between running from car and towards map edge
                    Vector3 finalTarget = Vector3.Lerp(targetDir, mapEdge, .5f);
    				
    				Quaternion targetRotation = Quaternion.LookRotation(finalTarget);
    				body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.deltaTime * rotationSpeed * dodgeRotationSpeedMultiplier);
					body.velocity = transform.forward * (speed * dodgeSpeedMultiplier * Time.fixedDeltaTime);
    			}else{
    				this.dodgeTimeoutRemaining = dodgeTimeout;
    				this.dodgeObject = null;
    				this.state = State.Idle;
    			}
    			break;
    		} 
			case(State.Igniting) : {
				if(tryDodgeTransition()){
					break;
				};
				this.animator.SetBool("Dodging", false);
				this.animator.SetBool("Igniting", true);
				RadioTower towerComponent = tower.GetComponent<RadioTower>();
				if(towerComponent == null){
					break;
				}

				attackTimeoutRemaining -= Time.deltaTime;
				if(attackTimeoutRemaining <= 0){
					towerComponent.hit(attackDamage);
					attackTimeoutRemaining = attackTimeout;
				}
    			break;
    		} case(State.Dead): {
    			this.transform.rotation = new Quaternion(80,30, 80, 1);
    			Destroy(body);
    			Destroy(this);
    			break;
    		}
    	}
    }

    void walkTowards(Vector3 goal) {
    	Vector3 targetDir = goal - body.position;
		targetDir.y = 0;
		Quaternion targetRotation = Quaternion.LookRotation(targetDir);
		body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		var vel = transform.forward * speed * Time.fixedDeltaTime;
		vel.y = body.velocity.y;
		body.velocity = vel;	
    }

    bool tryDodgeTransition () {
    	this.dodgeObject = getDodgeableObject();
		if(this.dodgeObject != null) {
			var rb = dodgeObject.GetComponent<Rigidbody>();
			if (rb == null || rb.velocity.magnitude > 10f) {
				this.dodgeTimeRemaining = dodgeTime;
				this.state = State.Dodge;
				this.animator.SetBool("Igniting", false);
				this.animator.SetBool("Dodging", true);
				return true;
			}
		}
		return false;
    }

    GameObject getDodgeableObject() {
    	if(hasCapabilityDodge) {
    		if(dodgeTimeoutRemaining > 0){
    			dodgeTimeoutRemaining -= Time.deltaTime;
    			return null;
    		}
    		if(dodgeObject != null){
    			return dodgeObject;
    		}
			var foundCanvasObjects = GameObject.FindGameObjectsWithTag("MobAvoid");
			foreach(var obj in foundCanvasObjects) {
				float dst = Vector3.Distance(obj.transform.position, body.position);
				if(dst < dodgeTriggerArea) {
					return obj;
				}
			}	
    	}
    	return null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (enabled && collision.gameObject.GetComponent<CarController>() != null)
        {
        	die();
        }

		// if (collision.gameObject.CompareTag("Boundary"))
		// {
		// 	Destroy(this.gameObject);
		// }
    }

    private void OnTriggerExit(Collider other) {
	    if (other.gameObject.CompareTag("Boundary")) {
		    Destroy(this.gameObject);

		    int numPoints = 2;
		    for (int i = 0; i < numPoints; i++) {
			    var point =Instantiate(PointPrefab, transform.position, Quaternion.identity);

			    float r = 1f;
			    point.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-r, r), 2f, Random.Range(-r, r));
            
			    Physics.IgnoreCollision(point.GetComponent<Collider>(), GetComponent<Collider>());
		    }
	    }
    }

    bool isOutOfBounds () {
    	return body.position.y < -1.0f || body.position.y > 10.0f;
    }

    public void die() {
	    if (enabled) {
		    this.state = State.Dead;
		    body.constraints = RigidbodyConstraints.None;
		    enabled = false;

		    if (deathEffect != null) {
			    Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
		    }
        
		    Destroy(gameObject, 2f);

		    
        
		    if (killSoundPrefab != null) {
			    var sound = Instantiate(killSoundPrefab, transform.position, Quaternion.identity);
			    Destroy(sound, 2f);
		    }
	    }
    }

}
