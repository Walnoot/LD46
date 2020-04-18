using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{

	enum State {
		Idle,
		Avoid,
		MoveToTarget,
		Igniting,
		Dead
	} 
	State state;
	private Rigidbody body;
    private float speed = 100.0f;
    private float rotationSpeed = 2f;
    private GameObject target;

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
    			if(target != null) {
    				Vector3 targetDir = target.transform.position - body.position;
    				targetDir.y = 0;;
    				// float targetAngle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);
    				Quaternion targetRotation = Quaternion.LookRotation(targetDir);
    				body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.deltaTime * 1);
					body.velocity = (transform.forward) * speed * Time.fixedDeltaTime;	
    			}
    			break;
    		}
    	}
    }

}
