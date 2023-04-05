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
    [SerializeField] private Button about_Btn;
    [SerializeField] private Button exit_Btn;

    [SerializeField] private GameObject levelWindow;
    [SerializeField] private GameObject aboutWindow;
    [SerializeField] private GameObject exitWindow;

    MyInput input;
    GameObject player;

    private void OnEnable()
    {
        input = GameManager.Instance.input;
        input.Disable();
        input.Menu.Enable();

        if (play_Btn) play_Btn.onClick.AddListener(OnPlay);
        if (about_Btn) about_Btn.onClick.AddListener(OnAbout);
        if (exit_Btn) exit_Btn.onClick.AddListener(OnExit);

        GetPlayer();

    }
    private void OnDisable()
    {
        if (play_Btn)  play_Btn.onClick.RemoveListener(OnPlay);
        if (about_Btn) about_Btn.onClick.RemoveListener(OnAbout);
        if (exit_Btn) exit_Btn.onClick.RemoveListener(OnExit);

        if (player != null) player.SetActive(false);
    }

    private void OnPlay()
    {
        DisableAllWindows();
        if (levelWindow) levelWindow.SetActive(true);
        player.GetComponent<Driver_Player>().SetSoundToHigh();
    }
    private void OnAbout()
    {
        DisableAllWindows();
        if (aboutWindow) aboutWindow.SetActive(true);
    }
    private void OnExit()
    {
        DisableAllWindows();
        if (exitWindow) exitWindow.SetActive(true);
    }
    private void DisableAllWindows()
    {
        if (levelWindow) levelWindow.SetActive(false);
        if (aboutWindow) aboutWindow.SetActive(false);
        if (exitWindow) exitWindow.SetActive(false);
    }

    private async void GetPlayer()
    {
        await GameManager.Instance.WaitForPlayer();
        player = GameManager.Instance.player;

        Transform playerTransform = player.GetComponent<Transform>();
        playerTransform.SetPositionAndRotation(playerStartTransform.position, playerStartTransform.rotation);
        playerTransform.parent = playerStartTransform;
    }
}
