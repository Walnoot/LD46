using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointLabel : MonoBehaviour {
    private Text text;

    private int curTextPoints = -999;
    private CarController car;
    void Start() {
        car = FindObjectOfType<CarController>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (car != null) {
            if (curTextPoints != car.Points) {
                curTextPoints = car.Points;
                text.text = curTextPoints.ToString();
            }
        }
    }
}
