using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    [Header("Vehicle parts")]
    [SerializeField] Engine engine;
    [SerializeField] GearBox gearBox;
    [SerializeField] Axle[] axles;
    [SerializeField] Transform steeringWheel;
    [SerializeField] float maxSteeringWheelAngle;

    [Header("Vehicle parameters")]
    [SerializeField] float downForce;
    [SerializeField] float downForceSpeed;

    Rigidbody m_RigidBody;
    Transform m_Transform;

    Vector3 velocity;
    Vector3 downForceVector;

    float steering;
    float acceleration;
    float braking;
    float handbraking;
    float clutch = 0;

    float speed;


    float m_AxleRPM;
    int m_MotorConnectedAxles;

    float m_GearBoxRPM;
    float m_EngineTorque;
    float m_GearBoxTorque;
    float m_MotorTorque;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();

        m_RigidBody.centerOfMass = Vector3.up * 0.2F;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) gearBox.ChangeDriveMode(DriveMode.DRIVE);
        if (Input.GetKeyDown(KeyCode.F)) gearBox.ChangeDriveMode(DriveMode.NUTERAL);
        if (Input.GetKeyDown(KeyCode.V)) gearBox.ChangeDriveMode(DriveMode.REVERCE);

        if (Input.GetKeyDown(KeyCode.LeftShift)) gearBox.GearUp();
        if(Input.GetKeyDown(KeyCode.LeftControl)) gearBox.GearDown();
        
        steering = Input.GetAxis("Horizontal");
        acceleration = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1F);
        braking = Mathf.Lerp(braking, (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? 1 : 0, 0.5F);
        if (Input.GetKeyDown(KeyCode.Space))
            handbraking = (handbraking > 0) ? 0 : 1;

        if (steeringWheel) UpdateSteering(steering);
    }

    private void UpdateSteering(float steering)
    {
        float steeringWheelAngle = steering * maxSteeringWheelAngle;
        Vector3 localRotation = steeringWheel.localEulerAngles;
        localRotation.y = steeringWheelAngle;
        steeringWheel.localEulerAngles = localRotation;
    }

    private void FixedUpdate()
    {
        velocity = m_RigidBody.velocity;
        velocity = transform.InverseTransformVector(velocity);

        speed = velocity.z * 3.6F;

        downForceVector.y = -downForce * (speed / downForceSpeed);
        m_RigidBody.AddForce(m_Transform.InverseTransformDirection(downForceVector));

        // Vehicle Section //

        m_AxleRPM = m_MotorConnectedAxles = 0;
        foreach(Axle axle in axles)
        {
            if (!axle.IsMotorConnecter) continue;

            m_MotorConnectedAxles++;
            m_AxleRPM = axle.GetWheelRPM();
        }
        m_AxleRPM = m_AxleRPM / m_MotorConnectedAxles;

        m_GearBoxRPM = gearBox.GetWheelRPM(m_AxleRPM);
        m_EngineTorque = engine.GetEngineTorque(m_GearBoxRPM, acceleration, clutch);
        m_GearBoxTorque = gearBox.GetGearBoxTorque(m_EngineTorque, clutch);

        foreach(Axle axle in axles)
        {
            axle.UpdatePhysics(steering, m_GearBoxTorque, braking, handbraking);
        }
    }
}