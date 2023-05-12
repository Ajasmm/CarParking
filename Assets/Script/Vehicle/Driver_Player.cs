using UnityEngine;
using Ajas.Vehicle;
using System.Threading;
using Cinemachine;
using Ajas.FrameWork;
using UnityEngine.InputSystem;
using System.Collections;
using System.Reflection;

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
        if (GameManager.Instance.player != null && GameManager.Instance.player != this.gameObject)
        {
            Destroy(this.gameObject);
            return;
        }
        else GameManager.Instance.RegisterPlayer(this.gameObject);

        vehicle.gearBox.Initilize();
        foreach (CinemachineVirtualCamera vcam in cameras) vcam.m_Priority = 0;
        cameras[currentCamera].m_Priority = 10;
        handBrake = 1;

        DontDestroyOnLoad(this.gameObject);
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
        float deltaTime = Time.deltaTime;
        if (input.GamePlay.enabled)
        {
#if UNITY_ANDROID
            wheelSteering = rawSteeringInput;
#else
            wheelSteering = Mathf.Lerp(wheelSteering, rawSteeringInput, steeringSencitivity * deltaTime);
#endif       
            pedalAcceleration = rawAccelrationInput;
        }
        else
        {
            wheelSteering = pedalAcceleration = 0;
            handBrake = 1;
        }

        acceleration = Mathf.Lerp(acceleration, (isGearChanging) ? gearAcceleration : pedalAcceleration,deltaTime * 5);

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
        clutch = 1;

        yield return new WaitForSeconds(0.25F);

        vehicle.gearBox.GearDown();

        clutch = 0;
        yield return new WaitForSeconds(0.25F);
       
        isGearChanging = false;
    }
    private IEnumerator GearUp()
    {
        isGearChanging = true;

        gearAcceleration = 0;
        clutch = 1;

        yield return new WaitForSeconds(0.25F);

        vehicle.gearBox.GearUp();

        clutch = 0;

        yield return new WaitForSeconds(0.25F);

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

    public void ResetPlayer()
    {
        // Do the task here
        sadgfsdgfsdfsd

        if (currentCamera != 0) SwitchCamera(new InputAction.CallbackContext());
    }

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
        GameManager.Instance?.UnRegisterPlayer(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.CurrentGamePlayMode != null && GameManager.Instance.CurrentGamePlayMode.isPlaying)
            GameManager.Instance.CurrentGamePlayMode.Failed();
    }
}
