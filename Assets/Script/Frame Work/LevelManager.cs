using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
            StartCoroutine(DoTransitionEffect());
            StartCoroutine(RegisterGameMode());
        }

        private IEnumerator RegisterGameMode()
        {
            GamePlayMode existingGamePlayMode = GameManager.Instance.CurrentGamePlayMode;
            if (existingGamePlayMode != null)
                Destroy(existingGamePlayMode.gameObject);

            yield return null;

            Resources.UnloadUnusedAssets();
            
            int currentLevel = GameManager.Instance.CurrentLevel;
            string levelName = "Assets/Levels/Level_" + currentLevel.ToString() + ".prefab";

            GameObject currentLevelPrefab;
            AsyncOperationHandle<GameObject> asyncOperation = Addressables.LoadAssetAsync<GameObject>(levelName);

            while (!asyncOperation.IsDone)
            {
                yield return null;
            }

            if(asyncOperation.Status == AsyncOperationStatus.Failed) 
            {
                yield break;
            }
            currentLevelPrefab = asyncOperation.Result;

            currentLevelPrefab = Instantiate(currentLevelPrefab, parkingArea, false);
            currentLevelPrefab.transform.localPosition = Vector3.zero;

            gamePlayMode = currentLevelPrefab.GetComponent<GamePlayMode>();

            if (gamePlayMode == null)
            {
                yield break;
            }

            gamePlayMode.SetGameWindows(gamePlay_UI, pauseMenu_UI, win_UI, fail_UI);
            yield return GameManager.Instance.WaitForPlayerEnumerator();
            GameManager.Instance.CurrentGamePlayMode = gamePlayMode;
        }

        public void RestartLevel()
        {
            GameManager.Instance.CurrentGamePlayMode?.OnStop();
            GameManager.Instance.CurrentGamePlayMode?.OnStart();
        }
        public void NextLevel()
        {
            GameManager.Instance.CurrentLevel = GameManager.Instance.CurrentLevel + 1;
            StartCoroutine(RegisterGameMode());
        }
        public void MainMenu()
        {
            SceneManager.LoadSceneAsync(1);
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