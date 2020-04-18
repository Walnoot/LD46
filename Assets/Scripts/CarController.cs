using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CarController : MonoBehaviour
{
    public WheelCollider FrontLeft, FrontRight, RearLeft, RearRight;

    public float Torque, BrakeTorque, SteerAngle;

    public float TargetSpeed;

    public int Points;

    public GameObject bulletPrefab;
    
    public float bulletsPerSecond, bulletSpeed;

    public List<TrailRenderer> trails;
    
    private Rigidbody rb;
    
    private Dictionary<UpgradeTree, int> upgrades = new Dictionary<UpgradeTree, int>();

    private float shootTimer;
    
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
        bool isBraking = Mathf.Abs(acceleration) > .1f && ((acceleration > 0f) != (speed > 0f));

        float absSpeed = Mathf.Abs(speed);

        float limitForce = 0f;
        
        if (absSpeed > TargetSpeed) {
            limitForce = (absSpeed - TargetSpeed) * 1000f;
        } else if (!isControlling) {
            limitForce = 1500f;
        }

        rb.AddForce(rb.velocity.normalized * -limitForce);

        float brakeTorque = isBraking ? BrakeTorque : 0f;
        float motorTorque = isBraking ? 0f : acceleration * Torque;
        
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
            rb.AddTorque(axis * (angle * 500f));
        }

        bool emitTrails = (isBraking || rb.angularVelocity.magnitude > 3f) && RearLeft.isGrounded && RearRight.isGrounded;

        foreach (var trail in trails) {
            trail.emitting = emitTrails;
        }

        if (bulletsPerSecond > 0f) {
            shootTimer -= Time.fixedDeltaTime;
            
            float dt = 1f / bulletsPerSecond;
            if (Input.GetButton("Fire") && shootTimer < 0f) {
                shootTimer = dt;

                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
                
                Destroy(bullet, 10f);
            }
        }
    }

    public int GetUpgradeLevel(UpgradeTree tree) {
        if (upgrades.ContainsKey(tree)) {
            return upgrades[tree];
        } else {
            return 0;
        }
    }

    public bool BuyUpgrade(UpgradeTree tree, Upgrade upgrade) {
        if (upgrade.Price > Points) {
            return false;
        } else {
            Points -= upgrade.Price;
            upgrades[tree] = GetUpgradeLevel(tree) + 1;

            if (upgrade.motorTorque > 0f) {
                Torque = upgrade.motorTorque;
            }

            if (upgrade.targetSpeed > 0f) {
                TargetSpeed = upgrade.targetSpeed;
            }
            
            return true;
        }
    }
}
