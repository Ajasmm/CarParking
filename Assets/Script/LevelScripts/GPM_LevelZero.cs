using Ajas.FrameWork;
using System.Collections;
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
    public override void OnPlay()
    {
        base.OnPlay();

        navigator.SetActive(true);
        input.GamePlay.Escape.performed += OnEscape;
    }

    public override void OnStart()
    {
        base.OnStart();

        navigator.SetActive(false);
        StartCoroutine(GetPlayer());
    }

    public override void OnStop()
    {
        base.OnStop();
        input.GamePlay.Escape.performed -= OnEscape;
         
        Rigidbody rbody;
        if (player && player.TryGetComponent<Rigidbody>(out rbody))
            rbody.velocity = Vector3.zero;
        
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

        if (director)
        {
            director.stopped -= OnTimeLineCompleted;
            Destroy(director);
        }

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
