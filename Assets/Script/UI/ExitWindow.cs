using Ajas.FrameWork;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ExitWindow : MonoBehaviour
{
    [SerializeField] private Button exit_Btn;
    [SerializeField] private Button cancel_Btn;

    private void OnEnable()
    {
        GameManager.Instance.input.Menu.Escape.performed +=  OnEscape;

        if(exit_Btn) exit_Btn.onClick.AddListener(OnExit);
        if (cancel_Btn) cancel_Btn.onClick.AddListener(OnCancel);
    }
    private void OnDisable()
    {
        GameManager.Instance.input.Menu.Escape.performed -=  OnEscape;

        if (exit_Btn) exit_Btn.onClick.RemoveListener(OnExit);
        if (cancel_Btn) cancel_Btn.onClick.RemoveListener(OnCancel);
    }
    private void OnEscape(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }

    private void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void OnCancel()
    {
        OnEscape(default(InputAction.CallbackContext));
    }
}

