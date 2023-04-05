using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ajas.FrameWork;
using System;
using UnityEngine.InputSystem;

public class GPM_LevelOne : GamePlayMode
{
    public override void OnStart()
    {
        Time.timeScale = 0F;
        input = GameManager.Instance.input;
        input.GamePlay.Escape.performed += EscapeMenu;
        DisableUI();
        parkingLot.Reset();

        GetPlayer();
    }
    public override void OnPlay()
    {
        isPlaying = true;
        Time.timeScale = 1;

        player.SetActive(true);
        input.GamePlay.Enable();
        if (gamePlay_UI) gamePlay_UI.SetActive(true);

        player.SetActive(true);
        player.GetComponent<Driver_Player>().SetSoundToHigh();
    }

    public override void OnPause()
    {
        isPlaying = false;
        Time.timeScale = 0;

        input.GamePlay.Disable();
        input.Menu.Enable();


        if (gamePlay_UI) gamePlay_UI.SetActive(false);
        if (pauseMenu_UI) pauseMenu_UI.SetActive(true);
        player.GetComponent<Driver_Player>().SetSoundToLow();
    }

    public override void OnResume()
    {
        isPlaying = true;
        Time.timeScale = 1;

        input.GamePlay.Enable();
        input.Menu.Disable();


        if (gamePlay_UI) gamePlay_UI.SetActive(true);
        if (pauseMenu_UI) pauseMenu_UI.SetActive(false);
        player.GetComponent<Driver_Player>().SetSoundToHigh();
    }

    public override void Won()
    {
        OnStop();
        if (win_UI) win_UI.SetActive(true);
    }

    public override void Failed()
    {
        OnStop();
        if (fail_UI) fail_UI.SetActive(true);
    }
    public override void OnStop()
    {
        isPlaying = false;
        Time.timeScale = 1;

        input?.GamePlay.Disable();
        input?.Menu.Enable();

        DisableUI();
        if (player) player.GetComponent<Driver_Player>().SetSoundToLow();
    }

    private void EscapeMenu(InputAction.CallbackContext context)
    {
        OnPause();
    }
    private async void GetPlayer()
    {
        await GameManager.Instance.WaitForPlayer();
        player = GameManager.Instance.player;

        player.transform.SetPositionAndRotation(playerStartPos.position, playerStartPos.rotation);
        Time.timeScale = 1F;
        OnPlay();
    }

    private void OnDestroy()
    {
        OnStop();
    }
}
