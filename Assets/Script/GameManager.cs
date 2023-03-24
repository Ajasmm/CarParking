using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static bool isGameEnded = false;
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (isGameEnded) return null;
            if (instance == null)
            {
                GameObject gameManagerObj = new GameObject("GameManager");
                gameManagerObj.SetActive(false);
                gameManagerObj.AddComponent<GameManager>();
                instance = gameManagerObj.GetComponent<GameManager>();
                gameManagerObj.SetActive(true);
            }
            return instance;
        }
        private set { instance = value; }
    }

    public GameObject player;

    private void OnEnable()
    {
        if(GameManager.Instance == null)
        {
            GameManager.Instance = this;
            isGameEnded = false;
            DontDestroyOnLoad(gameObject);
        }else if(GameManager.Instance != this) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(instance == this) isGameEnded = true;
    }

}
