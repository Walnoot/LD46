using System;
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

    public List<TrailRenderer> trails;

    public float boostInterval, boostTime, boostSpeed;

    public ParticleSystem exhaustParticles;

    public ParticleSystem.MinMaxGradient boostExhaustColor;

    [HideInInspector]
    public bool isBoosting;
    
    private Rigidbody rb;
    
    private Dictionary<UpgradeTree, int> upgrades = new Dictionary<UpgradeTree, int>();

    private float lastBoostTime = -999;

    private ParticleSystem.MinMaxGradient normalExhaustColor;

    private WheelCollider[] wheels;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (exhaustParticles != null) {
            normalExhaustColor = exhaustParticles.main.startColor;
        }
        
        wheels = new WheelCollider[] {FrontLeft, FrontRight, RearLeft, RearRight};
    }

    void FixedUpdate()
    {
        float acceleration = Input.GetAxisRaw("Vertical");
        float steering = Input.GetAxisRaw("Horizontal");

        float timeSinceBoost = Time.time - lastBoostTime;

        if (timeSinceBoost >= boostInterval && Input.GetButtonDown("Fire") && boostInterval > 0f) {
            lastBoostTime = Time.time;
            timeSinceBoost = 0f;

            rb.velocity += transform.forward * boostSpeed;

            if (exhaustParticles != null) {
                exhaustParticles.Emit(400);
            }
        }

        isBoosting = timeSinceBoost <= boostTime;
        
        float speed = Vector3.Dot(transform.forward, rb.velocity);

        bool isControlling = Mathf.Abs(acceleration) > .1f || Mathf.Abs(steering) > .1f;
        // bool isBreaking = !isControlling || (Mathf.Abs(acceleration) > .1f && ((acceleration > 0f) != (speed > 0f)));
        bool isBraking = Mathf.Abs(acceleration) > .1f && Mathf.Abs(speed) > .1f && ((acceleration > 0f) != (speed > 0f));

        float absSpeed = Mathf.Abs(speed);

        float limitForce = 0f;

        float curTargetSpeed = isBoosting ? TargetSpeed * 2f : TargetSpeed;
        
        if (absSpeed > curTargetSpeed) {
            limitForce = (absSpeed - curTargetSpeed) * 1000f;
        } else if (!isControlling) {
            limitForce = 1500f;
        }

        rb.AddForce(rb.velocity.normalized * -limitForce);

        float brakeTorque = isBraking ? BrakeTorque : 0f;
        float motorTorque = isBraking ? 0f : acceleration * Torque;

        if (isBoosting) {
            rb.AddForce(transform.forward * 1000f);
        }

        foreach (var wheel in wheels) {
            wheel.brakeTorque = brakeTorque;
            wheel.motorTorque = motorTorque;
        }

        FrontLeft.steerAngle = steering * SteerAngle;
        FrontRight.steerAngle = steering * SteerAngle;
        
        // self-righting
        var angle = Vector3.Angle(transform.up, Vector3.up);
        if(angle > 0.001)
        {
            var axis = Vector3.Cross(transform.up, Vector3.up);
            rb.AddTorque(axis * (angle * 500f));
        }

        bool emitTrails = (isBraking || rb.angularVelocity.magnitude > 3f || timeSinceBoost < .2f)
                          && RearLeft.isGrounded && RearRight.isGrounded;

        foreach (var trail in trails) {
            trail.emitting = emitTrails;
        }

        if (exhaustParticles != null) {
            var exhaustParticlesMain = exhaustParticles.main;
            exhaustParticlesMain.startColor = isBoosting ? boostExhaustColor : normalExhaustColor;
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

            if (upgrade.boostInterval > 0f) {
                boostInterval = upgrade.boostInterval;
            }

            if (upgrade.boostSpeed > 0f) {
                boostSpeed = upgrade.boostSpeed;
            }

            if (upgrade.steerAngle > 0f) {
                SteerAngle = upgrade.steerAngle;
            }

            if (upgrade.gripMultiplier > 0f) {
                foreach (var wheel in wheels) {
                    var fwdFriction = wheel.forwardFriction;
                    fwdFriction.stiffness = upgrade.gripMultiplier;
                    wheel.forwardFriction = fwdFriction;
                    
                    var sideFriction = wheel.sidewaysFriction;
                    sideFriction.stiffness = upgrade.gripMultiplier;
                    wheel.sidewaysFriction = sideFriction;
                }
            }

            if (upgrade.heal > 0) {
                foreach (var tower in FindObjectsOfType<RadioTower>()) {
                    tower.health += upgrade.heal;
                }
            }
            
            return true;
        }
    }
}
