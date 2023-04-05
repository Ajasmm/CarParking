using Ajas.FrameWork;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resume_Btn;
    [SerializeField] Button restart_Btn;
    [SerializeField] Button menu_Btn;

    [SerializeField] LevelManager level_Manager;

    MyInput input;

    private void OnEnable()
    {
        resume_Btn.onClick.AddListener(Resume);
        restart_Btn.onClick.AddListener(Restart);
        menu_Btn.onClick.AddListener(MainMenu);

        input = GameManager.Instance.input;
        input.Menu.Escape.performed += Escape;
     }
    private void OnDisable()
    {
        resume_Btn.onClick.RemoveListener(Resume);
        restart_Btn.onClick.RemoveListener(Restart);
        menu_Btn.onClick.RemoveListener(MainMenu);

        input.Menu.Escape.performed += Escape;
    }

    private void Resume()
    {
        GameManager.Instance.CurrentGamePlayMode?.OnResume();
    }
    private void Restart()
    {
        level_Manager.RestartLevel();
    }
    private void MainMenu()
    {
        level_Manager.MainMenu();
    }
    private void Escape(InputAction.CallbackContext context)
    {
        Resume();
    }
}
