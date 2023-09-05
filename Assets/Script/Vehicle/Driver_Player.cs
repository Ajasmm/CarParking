using UnityEngine;
using Ajas.Vehicle;
using Cinemachine;
using Ajas.FrameWork;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(Vehicle))]
public class Driver_Player : MonoBehaviour
{
    [Header("Vehicle Parameter")]
    [SerializeField] float steeringSencitivity = 1;
    [SerializeField] float pedalSencitivity = 2;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera[] cameras;

    [Header("Meter Data")]
    [SerializeField] MeterData meterData;

    int currentCamera = 0;

    float rawSteeringInput, rawAccelrationInput, rawBrakeInput, rawHandbrakeInput;

    float steering, acceleration, braking;
    float gearAcceleration, clutch;

    Vehicle vehicle;
    bool isGearChanging = false;

    MyInput input;

    private void Awake()
    {
        vehicle = GetComponent<Vehicle>();
    }

    private void Start()
    {
        vehicle.gearBox.Initilize();
        foreach (CinemachineVirtualCamera vcam in cameras) vcam.m_Priority = 0;
        cameras[currentCamera].m_Priority = 10;
        rawHandbrakeInput = 1;
        clutch = 0;
    }

    private void OnEnable()
    {
        input = GameManager.Instance.input;

        input.GamePlay.Camera.performed += SwitchCamera;

        input.GamePlay.Drive.performed += Drive;
        input.GamePlay.Nuteral.performed += Nuteral;
        input.GamePlay.Reverce.performed += Reverce;

        input.GamePlay.HandBrake.performed += HandBrake;
    }
    private void OnDisable()
    {
        input.GamePlay.Camera.performed -= SwitchCamera;

        input.GamePlay.Drive.performed -= Drive;
        input.GamePlay.Nuteral.performed -= Nuteral;
        input.GamePlay.Reverce.performed -= Reverce;

        input.GamePlay.HandBrake.performed -= HandBrake;
    }

    private void Update()
    {
        rawSteeringInput = input.GamePlay.Steering.ReadValue<float>();
        rawAccelrationInput = input.GamePlay.Acceleration.ReadValue<float>();
        rawBrakeInput = input.GamePlay.Brake.ReadValue<float>();

        // Raw Input
        float deltaTime = Time.deltaTime;
        if (input.GamePlay.enabled)
        {
            Controls.SteeringControlMode steeringControlMode = (Controls.SteeringControlMode) PlayerPrefs.GetInt(Controls.CONTROLLER_MODE);
            if(steeringControlMode == Controls.SteeringControlMode.Wheel)
            {
                steering = rawSteeringInput;
            }
            else
            {
                steering = Mathf.MoveTowards(steering, rawSteeringInput, deltaTime * steeringSencitivity);
            }

            acceleration = Mathf.MoveTowards(acceleration, rawAccelrationInput, deltaTime * pedalSencitivity);
            acceleration = Mathf.MoveTowards(
                acceleration, 
                (isGearChanging) ? gearAcceleration : rawAccelrationInput, 
                deltaTime * 5);

            braking = Mathf.MoveTowards(braking, rawBrakeInput, deltaTime * pedalSencitivity * 4);
        }
        else
        {
            steering = acceleration = 0;
            braking = 1;
        }


        vehicle.UpdateParameter(steering, acceleration, braking, rawHandbrakeInput, clutch);

        AutoGearChange();

        meterData.gear = vehicle.GetCurrentGear();
        meterData.speed = vehicle.GetSpeed();
        meterData.rpm = vehicle.GetEngineRPM();
    }
    private void AutoGearChange()
    {
        // Auto Gear
        GearBox gearBox = vehicle.gearBox;
        Engine engine = vehicle.engine;

        if (gearBox.driveMode != DriveMode.DRIVE) 
            return;

        Ratio currentRatio = gearBox.GetCurrentRatio();
        float currentRPM = engine.GetEngineRPM();
        float speed = vehicle.speed;

        if (currentRPM < gearBox.gearDownRPM && !isGearChanging)
        {
            if (gearBox.PeekGearDown())
            {
                StartCoroutine(GearDown());
            }
        } else if (speed > currentRatio.gearUpSpeed && !isGearChanging)
        {
            if (gearBox.PeekGearUp())
            {
                StartCoroutine(GearUp());
            }
        }
    }
    private IEnumerator GearDown()
    {
        isGearChanging = true;
        gearAcceleration = 0;

        const float waitTime = 0.25F;
        float tempWaitTime = waitTime;
        while(tempWaitTime > 0)
        {
            float deltaTime = Time.deltaTime;
            clutch = Mathf.MoveTowards(clutch, 1, deltaTime * (1 / waitTime));
            tempWaitTime -= deltaTime;
            
            yield return null;
        }
        clutch = 1;

        vehicle.gearBox.GearDown();

        tempWaitTime = waitTime;
        while (tempWaitTime > 0)
        {
            float deltaTime = Time.deltaTime;
            clutch = Mathf.MoveTowards(clutch, 0, deltaTime * (1 / waitTime));
            tempWaitTime -= deltaTime;

            yield return null;
        }
        clutch = 0;
        isGearChanging = false;
    }
    private IEnumerator GearUp()
    {
        isGearChanging = true;
        gearAcceleration = 0;

        const float waitTime = 0.25F;
        float tempWaitTime = waitTime;
        while (tempWaitTime > 0)
        {
            float deltaTime = Time.deltaTime;
            clutch = Mathf.MoveTowards(clutch, 1, deltaTime * (1 / waitTime));
            tempWaitTime -= deltaTime;

            yield return null;
        }
        clutch = 1;

        vehicle.gearBox.GearUp();

        tempWaitTime = waitTime;
        while (tempWaitTime > 0)
        {
            float deltaTime = Time.deltaTime;
            clutch = Mathf.MoveTowards(clutch, 0, deltaTime * (1 / waitTime));
            tempWaitTime -= deltaTime;

            yield return null;
        }
        clutch = 0;

        clutch = 0;
        isGearChanging = false;
    }


    private void Drive(InputAction.CallbackContext context) => vehicle.gearBox.ChangeDriveMode(DriveMode.DRIVE);
    private void Nuteral(InputAction.CallbackContext context) => vehicle.gearBox.ChangeDriveMode(DriveMode.NUTERAL);
    private void Reverce(InputAction.CallbackContext context) => vehicle.gearBox.ChangeDriveMode(DriveMode.REVERCE);

    private void SwitchCamera(InputAction.CallbackContext context)
    {
        cameras[currentCamera++].m_Priority = 0;
        if(currentCamera >= cameras.Length) currentCamera = 0;
        cameras[currentCamera].m_Priority = 10;

    }

    private void HandBrake(InputAction.CallbackContext context)
    {
        rawHandbrakeInput = (rawHandbrakeInput == 0) ? 1 : 0;
    }

    public void ResetPlayer()
    {
        // Do the task here
        rawHandbrakeInput = 1;
        if (currentCamera != 0)
            SwitchCamera(new InputAction.CallbackContext());
    }

    private void OnDestroy()
    {
        GameManager.Instance?.UnRegisterPlayer(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.CurrentGamePlayMode != null && GameManager.Instance.CurrentGamePlayMode.isPlaying)
            GameManager.Instance.CurrentGamePlayMode.Failed();
    }
}
