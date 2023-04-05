using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        [Header("Transition")]
        [SerializeField] float transitionTime = 2;
        [SerializeField] AnimationCurve fallOutEffect;
        [SerializeField] Image transitionImage;

        private GamePlayMode gamePlayMode;

        private void OnEnable()
        {
            RegisterGameMode();
        }

        private async void RegisterGameMode()
        {
            StartCoroutine(DoTransitionEffect());

            GamePlayMode existingGamePlayMode = GameManager.Instance.CurrentGamePlayMode;
            if (existingGamePlayMode != null)
            {
                Destroy(existingGamePlayMode.gameObject);
                Resources.UnloadUnusedAssets();
            }
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
            GameManager.Instance.CurrentLevel = GameManager.Instance.CurrentLevel + 1;
            RegisterGameMode();
        }
        public void MainMenu()
        {
            SceneManager.LoadSceneAsync(0);
        }

        IEnumerator DoTransitionEffect()
        {
            transitionImage.gameObject.SetActive(true);
            Color color = transitionImage.color;
            float time = 0;
            color.a = fallOutEffect.Evaluate(time);

            while(color.a > 0)
            {
                time += Time.unscaledDeltaTime * (1 / transitionTime);
                color.a = fallOutEffect.Evaluate(time);
                transitionImage.color = color;
                yield return null;
            }
            color.a = 0;
            transitionImage.color = color;
            transitionImage.gameObject.SetActive(false);
        }
    }
}