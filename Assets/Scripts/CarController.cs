using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    }
    public enum Axel
    {
        Front,
        Rear
    }
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }
    public ControlMode control;
    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;
    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;
    public Vector3 _centerOfMass;

    public List<Wheel> wheels;
    float moveInput;
    float steerInput;
    private Rigidbody carRb;
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }


    void Update()
    {
        GetInputs();
        AnimateWheel();
        WheelEffects();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }
    public void MoveInput(float Input)
    {
        moveInput = Input;
    }
    public void SteerInput(float Input)
    {
        steerInput = Input;
    }
    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }

    }
    void Move()
    {
        foreach (var Wheel in wheels)
        {
            Wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }
    void Steer()
    {
        foreach (var Wheel in wheels)
        {
            if (Wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                Wheel.wheelCollider.steerAngle = Mathf.Lerp(Wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }
    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var Wheel in wheels)
            {
                Wheel.wheelCollider.brakeTorque = 150 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var Wheel in wheels)
            {
                Wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }
    void AnimateWheel()
    {
        foreach (var Wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            Wheel.wheelCollider.GetWorldPose(out pos, out rot);
            Wheel.wheelModel.transform.position = pos;
            Wheel.wheelModel.transform.rotation = rot;
        }
    }
    void WheelEffects()
    {
        foreach (var Wheel in wheels)
        {
            if (Input.GetKey(KeyCode.Space) && Wheel.axel == Axel.Rear && Wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 1.0f)
            {
                Wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                Wheel.smokeParticle.Emit(1);
            }
            else
            {
                Wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;

            }
        }
    }
}
