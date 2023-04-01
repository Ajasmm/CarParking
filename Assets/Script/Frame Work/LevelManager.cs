using UnityEngine;

namespace Ajas.FrameWork
{
    public class LevelManager : MonoBehaviour
    {
        [Header("GamePlay")]
        [SerializeField] Transform parkingArea;

        [Header("UI")]
        [SerializeField] protected GameObject gamePlay_UI;
        [SerializeField] protected GameObject pauseMenu_UI;
        [SerializeField] protected GameObject win_UI;
        [SerializeField] protected GameObject fail_UI;

        private GamePlayMode gamePlayMode;

        private void OnEnable()
        {
            RegisterGameMode();
        }

        private async void RegisterGameMode()
        {
            GamePlayMode existingGamePlayMode = GameManager.Instance.CurrentGamePlayMode;
            if(existingGamePlayMode != null)
                Destroy(existingGamePlayMode.gameObject);

            int currentLevel = GameManager.Instance.CurrentLevel;
            string levelName = "Levels/Level_" + currentLevel.ToString();

            GameObject currentLevelPrefab = Resources.Load(levelName) as GameObject;
            if(currentLevelPrefab == null) Debug.Log(levelName + " Prefab is null");
            currentLevelPrefab = Instantiate(currentLevelPrefab, parkingArea, false);
            currentLevelPrefab.transform.localPosition = Vector3.zero;

            gamePlayMode = currentLevelPrefab.GetComponent<GamePlayMode>();
            if (gamePlayMode.isLastLvel) win_UI.GetComponent<WinScreen>().isLastLevel = true;
            win_UI.SetActive(false);
            win_UI.SetActive(true);

            gamePlayMode.SetGameWindows(gamePlay_UI,pauseMenu_UI,win_UI,fail_UI);

            if (gamePlayMode == null)
            {
                Debug.LogWarning("There is no gamemode. What you thinking. Am I a fool to you.");
                return;
            }

            await GameManager.Instance.WaitForPlayer();

            GameManager.Instance.CurrentGamePlayMode = gamePlayMode;
        }

        public void RestartLevel()
        {
            GameManager.Instance.CurrentGamePlayMode.OnStop();
            GameManager.Instance.CurrentGamePlayMode.OnStart();
        }
        public void NextLevel()
        {
            GameManager.Instance.CurrentLevel++;
            RegisterGameMode();
        }
        public void MainMenu()
        {
            Debug.Log("Sorry No MainMenu for Now");
        }
    }
}