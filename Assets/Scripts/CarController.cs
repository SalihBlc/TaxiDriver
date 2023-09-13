using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
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
        public Axel axel;
    }
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
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }
    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

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
                Wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
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
}
