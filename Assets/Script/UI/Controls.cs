using Ajas.FrameWork;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    public static readonly string CONTROLLER_MODE = "ControllerMode";

    [SerializeField] Button button;
    [SerializeField] Button steering;

    [SerializeField] Color selectedColor;
    [SerializeField] Color NoneSelectedColor;

    
    private void OnEnable()
    {
        GameManager.Instance.input.Menu.Escape.performed += OnEscape;
        UpdateSelectionColor();
    }
    private void OnDisable()
    {
        GameManager.Instance.input.Menu.Escape.performed -= OnEscape;
    }
    public void Exit()
    {
        OnEscape(default(InputAction.CallbackContext));
    }
    private void OnEscape(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }
    public void Button()
    {
        PlayerPrefs.SetInt(CONTROLLER_MODE, (int)SteeringControlMode.Button);
        UpdateSelectionColor();
    }
    public void Wheel()
    {
        PlayerPrefs.SetInt(CONTROLLER_MODE, (int)SteeringControlMode.Wheel);
        UpdateSelectionColor();
    }

    private void UpdateSelectionColor()
    {
        button.targetGraphic.color = NoneSelectedColor;
        steering.targetGraphic.color = NoneSelectedColor;

        switch ((SteeringControlMode)PlayerPrefs.GetInt(CONTROLLER_MODE))
        {
            case SteeringControlMode.Button:
                button.targetGraphic.color = selectedColor; break;
            case SteeringControlMode.Wheel:
                steering.targetGraphic.color = selectedColor; break;
        }
    }

    public enum SteeringControlMode
    {
        Wheel,
        Button
    }
}
