using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickup : MonoBehaviour {
    private float startTime;

    void Start() {
        startTime = Time.time;
    }

    private void OnCollisionEnter(Collision c) {
        float aliveTime = Time.time - startTime;
        if (aliveTime > .5f) {
            var controller = c.gameObject.GetComponent<CarController>();
            if (controller != null) {
                controller.Points += 1;
                Destroy(gameObject);
            }
        }
    }
}
