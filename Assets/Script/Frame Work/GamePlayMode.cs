using UnityEngine;

namespace Ajas.FrameWork
{
    public abstract class GamePlayMode : MonoBehaviour
    {
        [Header("GameMode default")]
        [SerializeField] protected Transform playerStartPos;
        [SerializeField] protected ParkingLot parkingLot;
        [SerializeField] public bool isPlaying = false;
        [SerializeField] public bool isLastLvel = false;

        [Header("UI")]
        protected GameObject gamePlay_UI; 
        protected GameObject pauseMenu_UI;
        protected GameObject win_UI;
        protected GameObject fail_UI;

        protected GameObject player;
        protected MyInput input;

        protected void OnEnable()
        {
            GameManager.Instance.OnPlayerChange += OnPlayerChange;
        }
        protected void OnDisable()
        {
            GameManager.Instance.OnPlayerChange -= OnPlayerChange;
        }

        public virtual void OnStart()
        {
            input = GameManager.Instance.input;
            input.Disable();
            DisableUI();

            player = GameManager.Instance.player;   
            player.GetComponent<Rigidbody>().isKinematic = false;
        }
        public virtual void OnStop()
        {
            Time.timeScale = 1;
            isPlaying = false;

            player.GetComponent<Rigidbody>().isKinematic = true;

            input.GamePlay.Disable();
            input.Menu.Enable();

            GameManager.Instance.SetVehicleSoundToLow();
            if(gamePlay_UI)
                gamePlay_UI.GetComponentInChildren<HandBrake>()?.SetHandBrake(true);
            DisableUI();
        }
        public virtual void OnPlay()
        {
            isPlaying = true;
            Time.timeScale = 1;

            input.GamePlay.Enable();
            DisableUI();

            if (gamePlay_UI)
            {
                gamePlay_UI.SetActive(true);
                gamePlay_UI.GetComponentInChildren<HandBrake>().SetHandBrake(true);
            }

            player.GetComponent<Driver_Player>().ResetPlayer();
            GameManager.Instance.SetVehicleSoundToHigh();
        }
        public virtual void OnPause()
        {
            isPlaying = false;
            Time.timeScale = 0f;

            input.Disable();
            input.Menu.Enable();

            DisableUI();
            if (pauseMenu_UI) pauseMenu_UI.SetActive(true);

            GameManager.Instance.SetVehicleSoundToLow();
        }
        public virtual void OnResume()
        {
            isPlaying = true;
            Time.timeScale = 1F;

            input.Disable();
            input.GamePlay.Enable();

            DisableUI();
            if (gamePlay_UI) gamePlay_UI.SetActive(true);
            GameManager.Instance.SetVehicleSoundToHigh();
        }
        public virtual void Won()
        {
            OnStop();
            DisableUI();
            if (win_UI)
                win_UI.SetActive(true);
        }
        public virtual void Failed() {
            OnStop();
            DisableUI();
            if (fail_UI)
                fail_UI.SetActive(true);
        }
        protected virtual void OnPlayerChange(GameObject player) {}

        public void SetGameWindows(GameObject gamePlay_UI, GameObject pauseMenu_UI, GameObject win_UI, GameObject fail_UI)
        {
            this.gamePlay_UI = gamePlay_UI;
            this.pauseMenu_UI = pauseMenu_UI;
            this.win_UI = win_UI;
            this.fail_UI = fail_UI;

            gamePlay_UI.GetComponentInChildren<HandBrake>().SetHandBrake(true);
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