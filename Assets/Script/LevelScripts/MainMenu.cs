using Ajas.FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerStartTransform;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Main Menu")]
    [SerializeField] private Button play_Btn;
    [SerializeField] private Button garage_Btn;
    [SerializeField] private Button controls_Btn;
    [SerializeField] private Button exit_Btn;

    [SerializeField] private GameObject levelWindow;
    [SerializeField] private GameObject garageWindow;
    [SerializeField] private GameObject controlsWindow;
    [SerializeField] private GameObject exitWindow;

    MyInput input;
    GameObject player;

    private void OnEnable()
    {
        input = GameManager.Instance.input;
        input.Disable();
        input.Menu.Enable();

        if (play_Btn) play_Btn.onClick.AddListener(OnPlay);
        if (garage_Btn) garage_Btn.onClick.AddListener(OnGarage);
        if (controls_Btn) controls_Btn.onClick.AddListener(OnControls);
        if (exit_Btn) exit_Btn.onClick.AddListener(OnExit);

        GameManager.Instance.LoadPlayer();
        GameManager.Instance.OnPlayerChange += OnPlayerChange;
        GetPlayer();

    }
    private void OnDisable()
    {
        if (play_Btn)  play_Btn.onClick.RemoveListener(OnPlay);
        if (garage_Btn) garage_Btn.onClick.RemoveListener(OnGarage);
        if (controls_Btn) controls_Btn.onClick.RemoveListener(OnControls);
        if (exit_Btn) exit_Btn.onClick.RemoveListener(OnExit);

        if (player != null) player.SetActive(false);
        GameManager.Instance.OnPlayerChange -= OnPlayerChange;
    }

    private void OnPlay()
    {
        DisableAllWindows();
        if (levelWindow) levelWindow.SetActive(true);
        player.GetComponent<Driver_Player>().SetSoundToHigh();
    }
    private void OnGarage()
    {
        DisableAllWindows();
        if(garageWindow) garageWindow.SetActive(true);
    }
    private void OnControls()
    {
        DisableAllWindows();
        if (controlsWindow) controlsWindow.SetActive(true);
    }
    private void OnExit()
    {
        DisableAllWindows();
        if (exitWindow) exitWindow.SetActive(true);
    }
    private void DisableAllWindows()
    {
        if (levelWindow) levelWindow.SetActive(false);
        if (garageWindow) garageWindow.SetActive(false);
        if (controlsWindow) controlsWindow.SetActive(false);
        if (exitWindow) exitWindow.SetActive(false);
    }

    private async void GetPlayer()
    {
        await GameManager.Instance.WaitForPlayer();
        player = GameManager.Instance.player;

        Transform playerTransform = player.GetComponent<Transform>();
        if(playerStartTransform) playerTransform.SetPositionAndRotation(playerStartTransform.position, playerStartTransform.rotation);
        playerTransform.parent = playerStartTransform;
    }
    private void OnPlayerChange(GameObject player)
    {
       GetPlayer();
    } 
}
