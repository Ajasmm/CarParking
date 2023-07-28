using Ajas.FrameWork;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] Button restart_Btn;
    [SerializeField] Button next_Btn;
    [SerializeField] Button mainMenu_Btn;

    [SerializeField] LevelManager levelManager;
    [SerializeField] public bool isLastLevel;

    private void OnEnable()
    {
        restart_Btn.onClick.AddListener(Restart);
        mainMenu_Btn.onClick.AddListener(MainMenu);

        GamePlayMode currentGameplayMode = GameManager.Instance.CurrentGamePlayMode.GetComponent<GamePlayMode>();
        if(currentGameplayMode != null && currentGameplayMode.isLastLvel && next_Btn != null) 
            Destroy(next_Btn.gameObject);
        else 
            next_Btn.onClick.AddListener(NextLevel);
    }
    private void OnDisable()
    {
        restart_Btn.onClick.RemoveListener(Restart);
        mainMenu_Btn.onClick.RemoveListener(MainMenu);

        if (!isLastLevel && next_Btn != null) next_Btn.onClick.RemoveListener(NextLevel);
    }

    private void NextLevel()
    {
        levelManager?.NextLevel();
    }
    private void Restart()
    {
        levelManager.RestartLevel();
    }
    private void MainMenu()
    {
        levelManager.MainMenu();
    }
}
