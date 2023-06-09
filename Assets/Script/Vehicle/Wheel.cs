using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ajas.Vehicle
{

    [RequireComponent(typeof(WheelCollider))]
    internal class Wheel : MonoBehaviour
    {
        WheelCollider wheelCollider;

        [SerializeField] Transform tyreMesh;
        [SerializeField] Transform BrakeMesh;

        Vector3 tyrePos;
        Quaternion TyreRot;
        Vector3 brakeRot = Vector3.zero;

        float steerAngle;
        float wheelRPM;

        public float SteerAngle
        {
            private get { return 0; }
            set
            {
                steerAngle = value;
                wheelCollider.steerAngle = value;
            }
        }
        public float MotorTorque
        {
            private get { return 0; }
            set
            {
                wheelCollider.motorTorque = value;
            }
        }
        public float BrakeTorue
        {
            private get { return 0; }
            set
            {
                wheelCollider.brakeTorque = value;
            }
        }

        private void Awake()
        {
            wheelCollider = GetComponent<WheelCollider>();
            wheelCollider.ConfigureVehicleSubsteps(3, 15, 20);
            wheelCollider.ResetSprungMasses();
        }

        // Update is called once per frame
        void Update()
        {
            wheelCollider.GetWorldPose(out tyrePos, out TyreRot);

            brakeRot.y = steerAngle;

            tyreMesh.SetPositionAndRotation(tyrePos, TyreRot);
            if (BrakeMesh)
            {
                BrakeMesh.position = tyrePos;
                BrakeMesh.localEulerAngles = brakeRot;
            }
        }

        public float GetWheelRPM()
        {
            wheelRPM = wheelCollider.rpm;
            wheelRPM = Mathf.Lerp(wheelRPM, wheelCollider.rpm, 0.5F);
            return wheelRPM;
        }
    }
}