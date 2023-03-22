using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ajas.Vehicle
{
    [RequireComponent(typeof(AudioSource))]
    internal class EngineSound : MonoBehaviour
    {
        [Header("For Debugging")]
        [SerializeField] bool isDebuging = false;
        [SerializeField] float d_Acceleration = 1;
        [SerializeField] float d_rpm = 1000;

        [Header("Sound parameters")]
        [SerializeField] float minRPM;
        [SerializeField] float minPitch;
        [SerializeField] float fadeInRPMRange;

        [SerializeField] float maxRPM;
        [SerializeField] float maxPitch;
        [SerializeField] float fadeOutRPMRange;

        [SerializeField] float deacceleratinVolume;

        AudioSource audioSource;

        float accelerationVolume, fadeVolume;
        public float rpmInRange, rpmRange;
        public float pitchValue, pitchRange;

        private void Awake()
        {
            rpmRange = maxRPM - minRPM;
            pitchRange = maxPitch - minPitch;

            audioSource = GetComponent<AudioSource>();
        }
        private void Update()
        {
            if (isDebuging)
            {
                H_UpdateSound(d_Acceleration, d_rpm);
            }
        }
        public void UpdateSound(float acceleration, float rpm)
        {
            if (isDebuging) return;
            H_UpdateSound(acceleration, rpm);
        }
        private void H_UpdateSound(float acceleration, float rpm)
        {
            audioSource.volume = CalculateVolume(acceleration, rpm);
            audioSource.pitch = CalculatePitch(rpm);
            if (!audioSource.isPlaying) audioSource.Play();
        }
        private float CalculatePitch(float rpm)
        {
            rpmInRange = rpm - minRPM;
            pitchValue = minPitch + ((rpmInRange / rpmRange) * pitchRange);
            return pitchValue;
        }

        private float CalculateVolume(float acceleration, float rpm)
        {
            accelerationVolume = (acceleration * (1 - deacceleratinVolume)) + deacceleratinVolume;

            if (rpm >= minRPM && rpm <= maxRPM) fadeVolume = 1;
            else if (rpm < (minRPM - fadeInRPMRange) || rpm > (maxRPM + fadeOutRPMRange)) fadeVolume = 0;
            else if (rpm < minRPM)
            {
                float factor;
                float rpmInRange = rpm - (minRPM - fadeInRPMRange);
                factor = rpmInRange / fadeInRPMRange;
                fadeVolume = factor;
            }
            else if (rpm > maxRPM)
            {
                float factor;
                float rpmInRange = (maxRPM + fadeOutRPMRange) - rpm;
                factor = rpmInRange / fadeInRPMRange;
                fadeVolume = factor;
            }

            if (fadeVolume < 0) fadeVolume = 0;

            return (fadeVolume < accelerationVolume) ? fadeVolume : accelerationVolume;
        }
    }
}