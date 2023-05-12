using System.Collections;
using UnityEngine;
using Ajas.FrameWork;
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

        StartCoroutine(GetPlayer());
    }
    public override void OnPlay()
    {
        isPlaying = true;
        Time.timeScale = 1;

        player.SetActive(true);
        input.GamePlay.Enable();
        DisableUI();
        if (gamePlay_UI) gamePlay_UI.SetActive(true);

        player.SetActive(true);
        player.GetComponent<Driver_Player>().ResetPlayer();
        GameManager.Instance.SetVehicleSoundToHigh();
    }

    public override void OnPause()
    {
        isPlaying = false;
        Time.timeScale = 0;

        input.GamePlay.Disable();
        input.Menu.Enable();


        if (gamePlay_UI) gamePlay_UI.SetActive(false);
        if (pauseMenu_UI) pauseMenu_UI.SetActive(true);
        GameManager.Instance.SetVehicleSoundToLow();
    }

    public override void OnResume()
    {
        isPlaying = true;
        Time.timeScale = 1;

        input.GamePlay.Enable();
        input.Menu.Disable();


        if (gamePlay_UI) gamePlay_UI.SetActive(true);
        if (pauseMenu_UI) pauseMenu_UI.SetActive(false);
        GameManager.Instance.SetVehicleSoundToHigh();
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

        GameManager.Instance.SetVehicleSoundToLow();
    }

    private void EscapeMenu(InputAction.CallbackContext context)
    {
        OnPause();
    }
    private IEnumerator GetPlayer()
    {
        yield return GameManager.Instance.WaitForPlayerEnumerator();
        player = GameManager.Instance.player;

        if(playerStartPos) player.transform.SetPositionAndRotation(playerStartPos.position, playerStartPos.rotation);
        Time.timeScale = 1F;
        OnPlay();
    }

    private void OnDestroy()
    {
        OnStop();
    }

    protected override void OnPlayerChange(GameObject player)
    {
        this.player = player;
        player.transform.SetPositionAndRotation(playerStartPos.position, playerStartPos.rotation);
    }
}
