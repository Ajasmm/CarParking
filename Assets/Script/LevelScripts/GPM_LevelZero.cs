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

        GameManager.Instance.SetVehicleSoundToLow();
    }

    public override void OnPlay()
    {
        isPlaying = true;
        Time.timeScale = 1;

        input.GamePlay.Enable();
        navigator.SetActive(true);

        DisableUI();
        if(gamePlay_UI) gamePlay_UI.SetActive(true);

        GameManager.Instance.SetVehicleSoundToHigh();

        input.GamePlay.Escape.performed += OnEscape;
        
        if (director) Destroy(director.gameObject);

        player.GetComponent<Driver_Player>().ResetPlayer();
    }

    public override void OnResume()
    {
        isPlaying = true;
        Time.timeScale = 1F;

        input.Disable();
        input.GamePlay.Enable();

        DisableUI();
        if (gamePlay_UI) gamePlay_UI.SetActive(true);
        GameManager.Instance.SetVehicleSoundToHigh();
    }

    public override void OnStart()
    {
        input = GameManager.Instance.input;
        input.Disable();

        navigator.SetActive(false);
        DisableUI();
        StartCoroutine(GetPlayer());
    }

    public override void OnStop()
    {
        Time.timeScale = 1;
        isPlaying = false;

        input.GamePlay.Disable();
        input.Menu.Enable();

        input.GamePlay.Escape.performed -= OnEscape;
        GameManager.Instance.SetVehicleSoundToLow();
    }

    public override void Won()
    {
        OnStop();
        DisableUI();
        if (win_UI) win_UI.SetActive(true);
    }

    private IEnumerator GetPlayer()
    {
        yield return GameManager.Instance.WaitForPlayerEnumerator();
        player = GameManager.Instance.player;

        player.transform.SetPositionAndRotation(playerStartPos.position, playerStartPos.rotation);
        player.SetActive(true);
        StartTimeLine();
        
    }
    private void StartTimeLine()
    {
        Time.timeScale = 1;

        input.TimeLine.Enable();
        input.TimeLine.Skip.performed += SkipTimeLine;

        if (director)
        {
            director.stopped += OnTimeLineCompleted;
            director.Play();
        }
        else OnTimeLineCompleted(null);
    }
    private void OnTimeLineCompleted(PlayableDirector director)
    {
        input.TimeLine.Disable();
        input.TimeLine.Skip.performed -= SkipTimeLine;

        if(director) director.stopped -=  OnTimeLineCompleted;

        OnPlay();
    }
    private void SkipTimeLine(InputAction.CallbackContext context)
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
