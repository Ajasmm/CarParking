using UnityEngine;
using Ajas.Vehicle;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Cinemachine;
using Ajas.FrameWork;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using System.Collections;

[RequireComponent(typeof(Vehicle))]
public class Driver_Player : MonoBehaviour
{
    [Header("Vehicle Parameter")]
    [SerializeField] float steeringSencitivity = 1;
    [SerializeField] AudioMixer audioMixer;

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
        if (GameManager.Instance.player != null && GameManager.Instance.player != this.gameObject) Destroy(this.gameObject);
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
            wheelSteering = Mathf.Lerp(wheelSteering, rawSteeringInput, steeringSencitivity * deltaTime);
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

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.CurrentGamePlayMode != null && GameManager.Instance.CurrentGamePlayMode.isPlaying)
            GameManager.Instance.CurrentGamePlayMode.Failed();
    }

    public void SetSoundToLow()
    {
        StartCoroutine(SetSoundToLowCoroutine());
    }
    public void SetSoundToHigh()
    {
        StartCoroutine(SetSoundToHighCoroutine());
    }
    private IEnumerator SetSoundToHighCoroutine()
    {
        float volume;
        audioMixer.GetFloat("Vehicle Volume", out volume);
        while (volume < 0)
        {
            volume += Time.unscaledDeltaTime * 80 * 2;
            if (volume > 0) volume = 0;
            audioMixer.SetFloat("Vehicle Volume", volume);
            yield return null;
        }
    }
    private IEnumerator SetSoundToLowCoroutine()
    {
        float volume;
        audioMixer.GetFloat("Vehicle Volume", out volume);
        while (volume > -80)
        {
            volume -= Time.unscaledDeltaTime * 80 * 2;
            if (volume < -80) volume = -80;
            audioMixer.SetFloat("Vehicle Volume", volume);
            yield return null;
        }
    }
}
