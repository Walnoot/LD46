using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private GameObject target;

    void Start() {
        target = FindObjectOfType<CarController>().gameObject;
    }

    void FixedUpdate()
    {
        if (target != null) {
            Vector3 diff = target.transform.position - transform.position;
            transform.position += diff * (Time.fixedDeltaTime * 10f);
        }
    }
}
