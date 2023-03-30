using UnityEngine;
using Ajas.Vehicle;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Cinemachine;

[RequireComponent(typeof(Vehicle))]
public class Driver_Player : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera[] cameras;
    int currentCamera = 0;

    float wheelSteering, pedalAcceleration, pedalBrake, handBrake;
    float acceleration;

    float gearAcceleration, clutch;

    Vehicle vehicle;
    bool isGearChanging = false;

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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) SwitchCamera(); 

        if (Input.GetKeyDown(KeyCode.R)) vehicle.gearBox.ChangeDriveMode(DriveMode.DRIVE);
        if (Input.GetKeyDown(KeyCode.F)) vehicle.gearBox.ChangeDriveMode(DriveMode.NUTERAL);
        if (Input.GetKeyDown(KeyCode.V)) vehicle.gearBox.ChangeDriveMode(DriveMode.REVERCE);

        if (Input.GetKeyDown(KeyCode.LeftShift) && vehicle.gearBox.PeekGearUp()) vehicle.gearBox.GearUp();
        if(Input.GetKeyDown(KeyCode.LeftControl) && vehicle.gearBox.PeekGearDown()) vehicle.gearBox.GearDown();

        // Raw Input
        wheelSteering = Input.GetAxis("Horizontal");
        pedalAcceleration = Mathf.Clamp01(Input.GetAxis("Vertical"));
        pedalBrake = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? 1 : 0;
        if(Input.GetKeyDown(KeyCode.Space)) handBrake = (handBrake == 0) ? 1 : 0;

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

    private void SwitchCamera()
    {
        cameras[currentCamera++].m_Priority = 0;
        if(currentCamera >= cameras.Length) currentCamera = 0;
        cameras[currentCamera].m_Priority = 10;

    }

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }
}
