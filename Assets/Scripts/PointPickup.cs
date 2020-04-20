using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickup : MonoBehaviour {
    public GameObject pickupEffect;
    
    private void OnTriggerEnter(Collider c) {
        var controller = c.gameObject.GetComponent<CarController>();
        if (controller != null) {
            controller.Points += 1;
            Destroy(gameObject);

            if (pickupEffect != null) {
                Instantiate(pickupEffect, transform.position, pickupEffect.transform.rotation);
            }
        }
    }
}
