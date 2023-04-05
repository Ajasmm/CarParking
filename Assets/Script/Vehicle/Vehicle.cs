using Ajas.FrameWork;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace Ajas.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class Vehicle : MonoBehaviour
    {
        [Header("Vehicle parts")]
        [SerializeField] internal Engine engine;
        [SerializeField] internal GearBox gearBox;
        [SerializeField] Axle[] axles;
        [SerializeField] Transform steeringWheel;
        [SerializeField] float maxSteeringWheelAngle;
        [SerializeField] VehicleMeter_Digital vehicleMeter;

        [Header("Vehicle parameters")]
        [SerializeField] float downForce;
        [SerializeField] float downForceSpeed;

        [Header("Engine Sound")]
        [SerializeField] EngineSound[] engineSounds;

        [Header("Lights")]
        [SerializeField] Material brakeMaterial;

        Rigidbody m_RigidBody;
        Transform m_Transform;

        Vector3 velocity;
        Vector3 downForceVector;

        float steering;
        float acceleration;
        float braking;
        float handbraking;
        float clutch = 0;

        public float speed { private set; get; }

        float prevSteering;

        float m_AxleRPM;
        int m_MotorConnectedAxles;

        float m_GearBoxRPM;
        float m_EngineTorque;
        float m_GearBoxTorque;
        float m_MotorTorque;

        int rpm;
        string currentGear;


        private void Awake()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Transform = GetComponent<Transform>();

            m_RigidBody.centerOfMass = Vector3.up * 0.2F;
        }
        private void Update()
        {

            rpm = (int)engine.GetEngineRPM();
            currentGear = gearBox.GetCurrentGear();

            if (steeringWheel) UpdateSteering(steering);
            foreach (EngineSound engineSound in engineSounds) engineSound.UpdateSound(acceleration, engine.GetEngineRPM());
            if (vehicleMeter) vehicleMeter.UpdateMeter(speed, rpm, currentGear);

            if (brakeMaterial != null && (handbraking > 0 || braking > 0))
                brakeMaterial.SetFloat("_Emission", 1);
            else
                brakeMaterial.SetFloat("_Emission", 0);
        }

        private void UpdateSteering(float steering)
        {
            float steeringWheelAngle = steering - prevSteering;
            steeringWheelAngle *= maxSteeringWheelAngle;
            prevSteering = steering;

            steeringWheel.Rotate(Vector3.up, steeringWheelAngle, Space.Self);
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
            foreach (Axle axle in axles)
            {
                if (!axle.IsMotorConnecter) continue;

                m_MotorConnectedAxles++;
                m_AxleRPM = axle.GetWheelRPM();
            }
            m_AxleRPM = m_AxleRPM / m_MotorConnectedAxles;

            m_GearBoxRPM = gearBox.GetWheelRPM(m_AxleRPM);
            m_EngineTorque = engine.GetEngineTorque(m_GearBoxRPM, acceleration, clutch);
            m_GearBoxTorque = gearBox.GetGearBoxTorque(m_EngineTorque, clutch);

            foreach (Axle axle in axles)
            {
                axle.UpdatePhysics(steering, m_GearBoxTorque / m_MotorConnectedAxles, braking, handbraking);
            }
        }

        public void UpdateParameter(float wheelSteering, float acceleration, float pedalBrake, float handBrake)
        {
            this.steering = wheelSteering;
            this.acceleration = acceleration;
            this.braking = pedalBrake;
            this.handbraking = handBrake;

            bool negativeSteering = (this.steering < 0) ? true : false;
            float steering;

            steering = (Mathf.Lerp(0.1F, 1, (50 - speed) / 50));
            if (Mathf.Abs(this.steering) > steering)
                this.steering = (negativeSteering) ? steering * -1 : steering;


        }

        private void OnDestroy()
        {
            if (GameManager.Instance?.player == this.gameObject) GameManager.Instance?.RegisterPlayer(null);
        }

        public int GetSpeed() { return (int) speed; }
        public int GetEngineRPM() { return rpm; }
        public string GetCurrentGear() { return currentGear; }  
    }
}