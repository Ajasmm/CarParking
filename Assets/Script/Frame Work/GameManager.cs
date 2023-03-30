using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if(isGameEnded) return null;
            if(instatnce == null)
            {
                GameObject gameManagerObj = new GameObject("GameManager");
                gameManagerObj.AddComponent<GameManager>();
            }
            return instatnce;
        }
        private set { }
    }
    private static GameManager instatnce;

    public GameObject player { private set; get; }
    private static bool isGameEnded = false;

    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public void OnEnable()
    {
        if(instatnce == null)
        {
            instatnce = this;
            DontDestroyOnLoad(gameObject);
        }else if(instatnce != this) Destroy(gameObject);
    }

    public void RegisterPlayer(GameObject player)
    {
        this.player = player;
    }
    public Task WaitForPlayer()
    {
        Task task = Task.Run(() =>
        {
            while(player == null)
            {
                if (cancellationTokenSource.Token.IsCancellationRequested) return;
                Debug.Log("Player is not registered!");
            }
        }, cancellationTokenSource.Token);
        return task;
    }

    private void OnDestroy()
    {
        if(instatnce == this) isGameEnded = true;
        cancellationTokenSource.Cancel();
    }
}
