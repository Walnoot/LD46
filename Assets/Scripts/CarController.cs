using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider FrontLeft, FrontRight, RearLeft, RearRight;

    public float Torque, BrakeTorque, SteerAngle;

    public float TargetSpeed;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float acceleration = Input.GetAxisRaw("Vertical");
        float steering = Input.GetAxisRaw("Horizontal");

        float speed = Vector3.Dot(transform.forward, rb.velocity);

        bool isBreaking = Mathf.Abs(acceleration) > .1f && ((acceleration > 0f) != (speed > 0f));
        // bool isBreaking = Mathf.Abs(acceleration) > .1f && Mathf.Abs(speed) > .5f && ((acceleration > 0f) != (speed > 0f));

        float absSpeed = Mathf.Abs(speed);

        if (absSpeed > TargetSpeed) {
            float limitForce = (absSpeed - TargetSpeed) * 1000f;
            rb.AddForce(rb.velocity.normalized * -limitForce);
        }

        float brakeTorque = isBreaking ? BrakeTorque : 0f;
        float motorTorque = isBreaking ? 0f : acceleration * Torque;
        
        FrontLeft.brakeTorque = brakeTorque;
        FrontRight.brakeTorque = brakeTorque;
        RearLeft.brakeTorque = brakeTorque;
        RearRight.brakeTorque = brakeTorque;
        FrontLeft.motorTorque = motorTorque;
        FrontRight.motorTorque = motorTorque;
        RearLeft.motorTorque = motorTorque;
        RearRight.motorTorque = motorTorque;

        FrontLeft.steerAngle = steering * SteerAngle;
        FrontRight.steerAngle = steering * SteerAngle;
    }
}
