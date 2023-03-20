using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axle : MonoBehaviour
{
    [Header("Axle Parameters")]
    [SerializeField] bool isMotorConnected = true;

    [Header("Wheels")]
    [SerializeField] float maxSteerAngle = 30F;
    [SerializeField] float brakeTorque = 500;
    [SerializeField] float handBrakeTorque = 500F;
    [SerializeField] Wheel[] wheels;

    float m_BrakeTorque;
    float m_HandBrakeTorque;
    float m_FinalBrake;
    float m_WheelRPM;
    

    public void UpdatePhysics(float steering, float motorTorque, float brakeTorque, float handBrakeTorque)
    {
        m_BrakeTorque = brakeTorque * this.brakeTorque;
        m_HandBrakeTorque = handBrakeTorque * this.handBrakeTorque;
        m_FinalBrake = (m_BrakeTorque > m_HandBrakeTorque) ? m_BrakeTorque : m_HandBrakeTorque;
        if (m_FinalBrake < 1) m_FinalBrake = 0;

        foreach (var wheel in wheels)
        {
            wheel.SteerAngle = steering * maxSteerAngle;
            wheel.MotorTorque = (isMotorConnected) ? motorTorque : 0;
            wheel.BrakeTorue = m_FinalBrake;
        }
    }

    public float GetWheelRPM()
    {
        m_WheelRPM = 0;
        foreach (Wheel wheel in wheels) m_WheelRPM += wheel.GetWheelRPM();

        return m_WheelRPM / wheels.Length;
    }
    public bool IsMotorConnecter { get { return isMotorConnected; } private set { } }
}
