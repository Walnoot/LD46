using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCut : MonoBehaviour
{
	public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime);
	}
}
