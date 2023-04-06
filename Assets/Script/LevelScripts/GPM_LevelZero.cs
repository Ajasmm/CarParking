using Ajas.FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GPM_LevelZero : GamePlayMode
{
    [SerializeField] PlayableDirector director;
    [SerializeField] GameObject navigator;

    public override void Failed()
    {
        
    }

    public override void OnPause()
    {
        isPlaying = false;
        Time.timeScale = 0f;
        input.Disable();
        input.Menu.Enable();

        DisableUI();
        if (pauseMenu_UI) pauseMenu_UI.SetActive(true);

        player.GetComponent<Driver_Player>().SetSoundToLow();
    }

    public override void OnPlay()
    {
        isPlaying = true;
        Time.timeScale = 1;

        Destroy(director.gameObject);
        input.GamePlay.Enable();
        navigator.SetActive(true);

        DisableUI();
        if(gamePlay_UI) gamePlay_UI.SetActive(true);
        player.GetComponent<Driver_Player>().SetSoundToHigh();
        input.GamePlay.Escape.performed += OnEscape;
    }

    public override void OnResume()
    {
        isPlaying = true;
        Time.timeScale = 1F;
        input.Disable();
        input.GamePlay.Enable();

        DisableUI();
        if (gamePlay_UI) gamePlay_UI.SetActive(true);
        player.GetComponent<Driver_Player>().SetSoundToHigh();
    }

    public override void OnStart()
    {
        input = GameManager.Instance.input;
        input.Disable();

        navigator.SetActive(false);
        DisableUI();
        GetPlayer();
    }

    public override void OnStop()
    {
        isPlaying = false;
        input.GamePlay.Disable();
        input.Menu.Enable();
        input.GamePlay.Escape.performed -= OnEscape;
        if (player) player.GetComponent<Driver_Player>().SetSoundToLow();
    }

    public override void Won()
    {
        OnStop();
        DisableUI();
        if (win_UI) win_UI.SetActive(true);
    }

    private async void GetPlayer()
    {
        await GameManager.Instance.WaitForPlayer();
        player = GameManager.Instance.player;

        player.transform.SetPositionAndRotation(playerStartPos.position, playerStartPos.rotation);
        player.SetActive(true);
        StartTimeLine();
        
    }
    private void StartTimeLine()
    {
        Time.timeScale = 1;

        input.TimeLine.Enable();
        input.TimeLine.Skip.performed += Context => SkipTimeLine();

        director.stopped += PlayableDirector => OnTimeLineCompleted();
    }
    private void OnTimeLineCompleted()
    {
        input.TimeLine.Disable();
        input.TimeLine.Skip.performed -= Context => SkipTimeLine();

        director.stopped -= PlayableDirector => OnTimeLineCompleted();

        OnPlay();
    }
    private void SkipTimeLine()
    {
        director.Stop();
    }
    private void OnEscape(InputAction.CallbackContext context)
    {
        OnPause();
    }

    protected override void OnPlayerChange(GameObject player)
    {
        this.player = player;
        player.transform.SetPositionAndRotation(playerStartPos.position, playerStartPos.rotation);
        player.SetActive(true);
    }
}
