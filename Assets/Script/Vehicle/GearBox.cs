using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ajas.Vehicle
{
    [CreateAssetMenu(menuName = "Vehicle/GearBox", fileName = "_GearBox", order = 0)]
    public class GearBox : ScriptableObject
    {
        [Header("GearBox Parameter")]
        [SerializeField] public float finalratio;
        [SerializeField] public float reverceRatio;
        [SerializeField] public Ratio[] ratios;

        [Header("Vehicle Parameters")]
        [SerializeField] public float tyreRadius = 0.5F;
        [SerializeField] public float gearUpRPM = 5000;
        [SerializeField] public float gearDownRPM = 2000;

        int currentGear = 0;
        float clutch = 0;
        public DriveMode driveMode { private set; get; } = DriveMode.DRIVE;
        
        float currentRatio;
        float gearBoxForce = 0;

        float m_GearBoxRPM;

        public float GetGearBoxTorque(float engineTorque, float clutch)
        {
            this.clutch = clutch;

            switch (driveMode)
            {
                case DriveMode.NUTERAL:
                    currentRatio = 0;
                    break;
                case DriveMode.DRIVE:
                    currentRatio = ratios[currentGear].ratio;
                    break;
                case DriveMode.REVERCE:
                    currentRatio = reverceRatio;
                    break;
            }

            gearBoxForce = currentRatio * engineTorque * finalratio * (1 - clutch);
            return gearBoxForce;
        }

        public bool PeekGearUp()
        {
            return (driveMode == DriveMode.DRIVE && currentGear < ratios.Length - 1) ? true : false;
        }
        public bool PeekGearDown()
        {
            return (driveMode == DriveMode.DRIVE && currentGear > 0) ? true : false;
        }
        public void GearUp()
        {
            if (PeekGearUp()) ++currentGear;
        }
        public void GearDown()
        {
            if (PeekGearDown()) --currentGear;
        }

        public void ChangeDriveMode(DriveMode driveMode)
        {
            if (driveMode == this.driveMode) return;
            this.driveMode = driveMode;
            currentGear = 0;
        }
        public float GetWheelRPM(float axilRPM)
        {
            switch (driveMode)
            {
                case DriveMode.NUTERAL:
                    currentRatio = 0;
                    break;
                case DriveMode.DRIVE:
                    currentRatio = ratios[currentGear].ratio;
                    break;
                case DriveMode.REVERCE:
                    currentRatio = reverceRatio;
                    break;
            }
            float tempRPM;
            tempRPM = axilRPM * finalratio * currentRatio;
            tempRPM = (Mathf.Lerp(tempRPM, gearDownRPM, clutch));
            m_GearBoxRPM = Mathf.Lerp(m_GearBoxRPM, tempRPM, 0.5F);
            return m_GearBoxRPM;
        }

        public string GetCurrentGear()
        {
            switch (driveMode)
            {
                case DriveMode.NUTERAL:
                    return "N";
                case DriveMode.REVERCE:
                    return "R";
                case DriveMode.DRIVE:
                    return "D" + (currentGear + 1).ToString();
                default:
                    return " ";
            }
        }

        internal void Initilize()
        {
            ChangeDriveMode(DriveMode.DRIVE);
        }
        public Ratio GetCurrentRatio()
        {
            if (driveMode != DriveMode.DRIVE) return null;

            return ratios[currentGear];
        }
    }

    public enum DriveMode
    {
        NUTERAL,
        DRIVE,
        REVERCE
    }

    [Serializable]
    public class Ratio
    {
        public float ratio;
        public float gearDownSpeed;
        public float gearUpSpeed;
    }
}