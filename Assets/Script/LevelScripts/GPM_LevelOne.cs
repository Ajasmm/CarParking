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
        input.GamePlay.Enable();
        if (gamePlay_UI) gamePlay_UI.SetActive(true);
    }

    public override void OnPause()
    {
        input.GamePlay.Disable();
        input.Menu.Enable();

        Time.timeScale = 0;

        if (gamePlay_UI) gamePlay_UI.SetActive(false);
        if (pauseMenu_UI) pauseMenu_UI.SetActive(true);
    }

    public override void OnResume()
    {
        input.GamePlay.Enable();
        input.Menu.Disable();

        Time.timeScale = 1;

        if (gamePlay_UI) gamePlay_UI.SetActive(true);
        if (pauseMenu_UI) pauseMenu_UI.SetActive(false);
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
        input?.GamePlay.Disable();
        input?.Menu.Enable();

        DisableUI();
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
