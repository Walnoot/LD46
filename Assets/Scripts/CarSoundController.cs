using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundController : MonoBehaviour {
    public WheelCollider wheel;

    public float idlePitch, pitchOverTorque;
    public float changeTime;

    public float brakeVolumeOverSpeed;
    
    public AudioSource engineAudio, brakeAudio;

    private float blend;

    private Rigidbody rb;

    private float brakeVolume;
    
    private void Start() {
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update() {
        float target = Mathf.Abs(wheel.motorTorque / pitchOverTorque);
        float diff = target - blend;
        float dt = Time.deltaTime / changeTime;
        blend += Mathf.Clamp(diff, -dt, dt);

        engineAudio.pitch = idlePitch + blend;

        float speed = rb.velocity.magnitude;

        bool playBrake = rb.angularVelocity.magnitude > 3f || wheel.brakeTorque > 0f;
        
        float targetBrakeVolume = playBrake ? speed / brakeVolumeOverSpeed : 0f;
        brakeVolume += (targetBrakeVolume - brakeVolume) * Time.deltaTime * 10f;
        brakeAudio.volume = brakeVolume;
    }
}
