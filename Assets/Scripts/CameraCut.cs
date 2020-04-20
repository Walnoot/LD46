using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCut : MonoBehaviour
{
	public Transform target;

    void Update()
    {
		transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime);
	}
}
