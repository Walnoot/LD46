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

        bool isControlling = Mathf.Abs(acceleration) > .1f || Mathf.Abs(steering) > .1f;
        // bool isBreaking = !isControlling || (Mathf.Abs(acceleration) > .1f && ((acceleration > 0f) != (speed > 0f)));
        bool isBreaking = Mathf.Abs(acceleration) > .1f && ((acceleration > 0f) != (speed > 0f));

        float absSpeed = Mathf.Abs(speed);

        float limitForce = 0f;
        
        if (absSpeed > TargetSpeed) {
            limitForce = (absSpeed - TargetSpeed) * 1000f;
        } else if (!isControlling) {
            limitForce = 1500f;
        }

        rb.AddForce(rb.velocity.normalized * -limitForce);

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
        
        // self-righting
        var angle = Vector3.Angle(transform.up, Vector3.up);
        if(angle > 0.001)
        {
            var axis = Vector3.Cross(transform.up, Vector3.up);
            rb.AddTorque(axis * (angle * 1000f));
        }
    }
}
