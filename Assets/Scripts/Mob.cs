using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{

	enum State {
		Idle,
		Dodge,
		MoveToTarget,
		Igniting,
		Dead
	} 
	State state;

	public bool hasCapabilityDodge = true;
	public float dodgeTriggerArea = 4f; //Distance to player
	public float dodgeSpeedMultiplier = 2f; 
	public float dodgeRotationSpeedMultiplier = 3f; 
	public float dodgeTime = 2f; // How long dodge lasts
	public float dodgeTimeout = 4f; // Desired time between dodges 

	public GameObject deathEffect;

	public GameObject PointPrefab;
	
	private float dodgeTimeRemaining = 0f; 
	private float dodgeTimeoutRemaining = 2f;

	private Rigidbody body;
    private float speed = 100.0f;
    private float rotationSpeed = 1f;
    private GameObject target;
    private GameObject dodgeObject;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        target = null; //new Vector3(0,0,0);
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
    	switch(state) {
    		case(State.Idle):{
    			var foundCanvasObjects = GameObject.FindGameObjectsWithTag("MobTarget");
    			if(foundCanvasObjects.Length > 0) {
	    			var target = foundCanvasObjects[0];
	    			this.target = target;
	    			state = State.MoveToTarget;
    			}
    			break;
    		} case(State.MoveToTarget):{
    			this.dodgeObject = checkDodgeableObject();
    			if(this.dodgeObject != null) {
    				this.dodgeTimeRemaining = dodgeTime;
	    			this.state = State.Dodge;
    				break;
    			}
    			if(target != null) {
    				Vector3 targetDir = target.transform.position - body.position;
    				targetDir.y = 0;
    				Quaternion targetRotation = Quaternion.LookRotation(targetDir);
    				body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.deltaTime * rotationSpeed);
					body.velocity = transform.forward * speed * Time.fixedDeltaTime;	
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
    				Quaternion targetRotation = Quaternion.LookRotation(targetDir);
    				body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.deltaTime * rotationSpeed * dodgeRotationSpeedMultiplier);
					body.velocity = transform.forward * speed * dodgeSpeedMultiplier * Time.fixedDeltaTime;	
    			}else{
    				this.dodgeTimeoutRemaining = dodgeTimeout;
    				this.dodgeObject = null;
    				this.state = State.MoveToTarget;
    			}
    			break;
    		} 
			case(State.Igniting) : {
    			break;
    		} case(State.Dead): {
    			this.transform.rotation = new Quaternion(80,30, 80, 1);
    			Destroy(body);
    			Destroy(this);
    			break;
    		}
    	}
    }

    GameObject checkDodgeableObject() {
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
        	this.state = State.Dead;
            body.constraints = RigidbodyConstraints.None;
            enabled = false;

            if (deathEffect != null) {
	            Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
            }
            
            Destroy(gameObject, 60f);

            int numPoints = Random.Range(1, 3);
            for (int i = 0; i < numPoints; i++) {
	            var point =Instantiate(PointPrefab, transform.position, Quaternion.identity);

	            float r = 1f;
	            point.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-r, r), 2f, Random.Range(-r, r));
	            
	            Physics.IgnoreCollision(point.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
    }

}
