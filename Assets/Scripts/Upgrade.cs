using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/Upgrade", order = 1)]
[Serializable]
public class Upgrade : ScriptableObject {
    public int Price;

    public float motorTorque;
    public float targetSpeed;
    
    public float boostInterval;
    public float boostSpeed;

    public float steerAngle;

    public float gripMultiplier;

    public int heal;

    public bool isWarModel;
}
