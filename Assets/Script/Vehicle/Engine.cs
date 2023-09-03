using System;
using UnityEngine;

namespace Ajas.Vehicle
{
    [CreateAssetMenu(fileName = "_Engine", menuName = "Vehicle/Engine", order = 0)]
    public class Engine : ScriptableObject
    {
        [SerializeField] private AnimationCurve torqueCurve;
        [SerializeField] private AnimationCurve oppositionTorque;
        [SerializeField] private float minRPM;
        [SerializeField] private float maxRPM;
        [SerializeField] private float idleRPM;
        [SerializeField] private float accelerationRPM;

        float engineRPM;
        float currentRPM;
        float engineTorque;

        public float GetEngineTorque(float gearBoxRPM, float acceleration, float clutch)
        {
            engineRPM = Mathf.Lerp(idleRPM, accelerationRPM, acceleration);

            if (gearBoxRPM < minRPM)
                gearBoxRPM = minRPM;
            if (engineRPM < minRPM)
                engineRPM = minRPM;

            currentRPM = Mathf.Lerp(gearBoxRPM, engineRPM, clutch);

            engineTorque = torqueCurve.Evaluate(currentRPM) * acceleration;
            engineTorque -= oppositionTorque.Evaluate(currentRPM) * (1 - clutch) * (1 - acceleration);
            return engineTorque;
        }
        public float GetEngineRPM() 
        { 
            return (currentRPM < minRPM) ? minRPM : currentRPM; 
        }
    }
}