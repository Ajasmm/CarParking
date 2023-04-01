using Ajas.FrameWork;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : MonoBehaviour
{
    [SerializeField] Button restart_Btn;
    [SerializeField] Button mainMenu_Btn;

    [SerializeField] LevelManager levelManager;

    private void OnEnable()
    {
        restart_Btn.onClick.AddListener(Restart);
        mainMenu_Btn.onClick.AddListener(MainMenu);
    }
    private void OnDisable()
    {
        restart_Btn.onClick.RemoveListener(Restart);
        mainMenu_Btn.onClick.RemoveListener(MainMenu);
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
