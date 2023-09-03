using System.Collections;
using UnityEngine;
using Ajas.FrameWork;
using UnityEngine.InputSystem;

public class GPM_LevelOne : GamePlayMode
{
    public override void OnStart()
    {
        base.OnStart();
        input.GamePlay.Escape.performed += EscapeMenu;
        parkingLot.Reset();

        StartCoroutine(GetPlayer());
    }
    public override void OnStop()
    {
        base.OnStop();
        input.GamePlay.Escape.performed -= EscapeMenu;
    }
    public override void OnPlay()
    {
        base.OnPlay();
        player.SetActive(true);
    }

    private void EscapeMenu(InputAction.CallbackContext context)
    {
        OnPause();
    }
    private IEnumerator GetPlayer()
    {
        yield return GameManager.Instance.WaitForPlayerEnumerator();
        player = GameManager.Instance.player;

        if(playerStartPos)
            player.transform.SetPositionAndRotation(playerStartPos.position, playerStartPos.rotation);

        Time.timeScale = 1F;
        OnPlay();
    }

    private void OnDestroy()
    {
        OnStop();
    }

    protected override void OnPlayerChange()
    {
        this.player = GameManager.Instance.player;
        player.transform.SetPositionAndRotation(playerStartPos.position, playerStartPos.rotation);
    }
}
