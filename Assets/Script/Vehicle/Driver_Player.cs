using UnityEngine;
using Ajas.Vehicle;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Cinemachine;
using Ajas.FrameWork;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Vehicle))]
public class Driver_Player : MonoBehaviour
{
    [Header("Vehicle Parameter")]
    [SerializeField] float steeringSencitivity = 1;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera[] cameras;
    
    int currentCamera = 0;

    float rawSteeringInput, rawAccelrationInput;
    float wheelSteering, pedalAcceleration, pedalBrake, handBrake;
    float acceleration;

    float gearAcceleration, clutch;

    Vehicle vehicle;
    bool isGearChanging = false;

    MyInput input;
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    private void Awake()
    {
        vehicle = GetComponent<Vehicle>();
    }

    private void Start()
    {
        vehicle.gearBox.Initilize();
        foreach(CinemachineVirtualCamera vcam in cameras)  vcam.m_Priority = 0;
        cameras[currentCamera].m_Priority = 10;
        handBrake = 1;
    }

    private void OnEnable()
    {
        input = GameManager.Instance.input;
        input.Enable();

        input.GamePlay.Camera.performed += SwitchCamera;

        input.GamePlay.Drive.performed += Drive;
        input.GamePlay.Nuteral.performed += Nuteral;
        input.GamePlay.Reverce.performed += Reverce;

        input.GamePlay.Brake.performed += Brake;
        input.GamePlay.Brake.canceled += Brake;

        input.GamePlay.HandBrake.performed += HandBrake;
    }
    private void OnDisable()
    {
        input.GamePlay.Camera.performed -= SwitchCamera;

        input.GamePlay.Drive.performed -= Drive;
        input.GamePlay.Nuteral.performed -= Nuteral;
        input.GamePlay.Reverce.performed -= Reverce;

        input.GamePlay.Brake.performed -= Brake;
        input.GamePlay.Brake.canceled -= Brake;

        input.GamePlay.HandBrake.performed -= HandBrake;
    }

    private void Update()
    {
        rawSteeringInput = input.GamePlay.Steering.ReadValue<float>();
        rawAccelrationInput = input.GamePlay.Acceleration.ReadValue<float>();

        // Raw Input
        if (input.GamePlay.enabled)
        {
            float deltaTime = Time.deltaTime;
            wheelSteering = Mathf.Lerp(wheelSteering, rawSteeringInput, steeringSencitivity * deltaTime);
            pedalAcceleration = Mathf.Lerp(pedalAcceleration, rawAccelrationInput, steeringSencitivity * deltaTime);

            if (Mathf.Abs(0 - pedalAcceleration) < steeringSencitivity * deltaTime) pedalAcceleration = 0;
        }
        else
        {
            wheelSteering = pedalAcceleration = 0;
            handBrake = 1;
        }

        acceleration = Mathf.Lerp(acceleration, (isGearChanging) ? gearAcceleration : pedalAcceleration, 0.1F);

        vehicle.UpdateParameter(wheelSteering, acceleration, pedalBrake, handBrake);
    }
    private void FixedUpdate()
    {
        // Auto Gear
        GearBox gearBox = vehicle.gearBox;
        Engine engine = vehicle.engine;

        if (gearBox.driveMode != DriveMode.DRIVE) return;

        Ratio currentRatio = gearBox.GetCurrentRatio();
        float currentRPM = engine.GetEngineRPM();
        float speed = vehicle.speed;

        if(currentRPM < gearBox.gearDownRPM && !isGearChanging)
        {
            if (gearBox.PeekGearDown())
            {
                GearDown();
            }
        }else if(speed > currentRatio.gearUpSpeed && !isGearChanging)
        {
            if (gearBox.PeekGearUp())
            {
                GearUp();
            }
        }
    }
    private async void GearDown()
    {
        isGearChanging = true;
        double timeGap;
        Stopwatch stopwatch = new Stopwatch();
        gearAcceleration = acceleration;

        await Task.Run(() =>
        {
            while(gearAcceleration > 0 || clutch < 1)
            {
                timeGap = stopwatch.Elapsed.TotalSeconds * 8;
                stopwatch.Restart();

                if (gearAcceleration > 0) gearAcceleration -= (float)timeGap;
                if (gearAcceleration < 0) gearAcceleration = 0;

                if(clutch < 1) clutch += (float)timeGap;
                if (clutch > 1) clutch = 1;
                
                if (gearAcceleration == 0 && clutch == 1) break;

                if (cancellationTokenSource.IsCancellationRequested) return;
            }

            vehicle.gearBox.GearDown();
            

            while (clutch > 0)
            {
                timeGap = stopwatch.Elapsed.TotalSeconds * 8;
                stopwatch.Restart();

                gearAcceleration = Mathf.Lerp(gearAcceleration, pedalAcceleration, 0.1F);

                if(clutch > 0) clutch -= (float) timeGap;
                if(clutch < 0) clutch = 0;
                if (clutch == 0) break;

                if (cancellationTokenSource.IsCancellationRequested) return;
            }


        }, cancellationTokenSource.Token);
        isGearChanging = false;
    }
    private async void GearUp()
    {
        isGearChanging = true;
        double timeGap;
        Stopwatch stopwatch = new Stopwatch();
        gearAcceleration = acceleration;

        await Task.Run(() =>
        {
            while (gearAcceleration > 0 || clutch < 1)
            {
                timeGap = stopwatch.Elapsed.TotalSeconds * 4;
                stopwatch.Restart();

                if (gearAcceleration > 0) gearAcceleration -= (float)timeGap;
                if (gearAcceleration < 0) gearAcceleration = 0;

                if (clutch < 1) clutch += (float)timeGap;
                if (clutch > 1) clutch = 1;

                if (gearAcceleration == 0 && clutch == 1) break;

                if (cancellationTokenSource.IsCancellationRequested) return;
            }

            vehicle.gearBox.GearUp();

            while (clutch > 0)
            {
                timeGap = stopwatch.Elapsed.TotalSeconds * 2;
                stopwatch.Restart();

                gearAcceleration = Mathf.Lerp(gearAcceleration, pedalAcceleration, ((float)timeGap) * 2);

                if(clutch > 0) clutch -= (float)timeGap;
                if (clutch < 0) clutch = 0;
                if (clutch == 0) break;

                if (cancellationTokenSource.IsCancellationRequested) return;
            }


        }, cancellationTokenSource.Token);
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

    private void Brake(InputAction.CallbackContext context)
    {
        if (context.performed)
            pedalBrake = 1;
        else if (context.canceled)
            pedalBrake = 0;
    }
    private void HandBrake(InputAction.CallbackContext context)
    {
        handBrake = (handBrake != 0) ? 0 : 1;
    }

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.CurrentGamePlayMode.Failed();
    }
}
