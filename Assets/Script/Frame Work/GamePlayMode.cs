using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ajas.FrameWork
{
    public abstract class GamePlayMode : MonoBehaviour
    {
        [Header("GameMode default")]
        [SerializeField] protected Transform playerStartPos;
        [SerializeField] protected ParkingLot parkingLot;

        [SerializeField] public bool isLastLvel = false;

        [Header("UI")]
        [SerializeField] protected GameObject gamePlay_UI; 
        [SerializeField] protected GameObject pauseMenu_UI;
        [SerializeField] protected GameObject win_UI;
        [SerializeField] protected GameObject fail_UI;

        protected GameObject player;
        protected MyInput input;

        public abstract void OnStart();
        public abstract void OnStop();
        public abstract void OnPlay();
        public abstract void OnPause();
        public abstract void OnResume();
        public abstract void Won();
        public abstract void Failed();

        public void SetGameWindows(GameObject gamePlay_UI, GameObject pauseMenu_UI, GameObject win_UI, GameObject fail_UI)
        {
            this.gamePlay_UI = gamePlay_UI;
            this.pauseMenu_UI = pauseMenu_UI;
            this.win_UI = win_UI;
            this.fail_UI = fail_UI;
        }
        protected void DisableUI()
        {
            if(gamePlay_UI) gamePlay_UI.SetActive(false);
            if (pauseMenu_UI) pauseMenu_UI.SetActive(false);
            if (win_UI) win_UI.SetActive(false);
            if (fail_UI) fail_UI.SetActive(false);
        }
    }
}