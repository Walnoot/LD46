using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        var controller = other.GetComponent<CarController>();
        if (controller != null) {
            controller.Points += 1;
            Destroy(gameObject);
        }
    }
}
