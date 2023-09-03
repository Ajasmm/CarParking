using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class UI_ProgressiveButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float duration = 0.5F;

    [InputControl(layout = "Axis")]
    [SerializeField]
    private string m_ControlPath;


    bool isPointerDown = false;
    float value = 0;

    protected override string controlPathInternal { 
        get => m_ControlPath; 
        set => m_ControlPath = value; }

    private void Update()
    {
        if (isPointerDown) 
            value = Mathf.MoveTowards(value, 1, (1 / duration) * Time.deltaTime);
        else 
            value = Mathf.MoveTowards(value, 0, (1 / duration) * Time.deltaTime);

        SendValueToControl(value);
    }

    public float GetValue()
    {
        return value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }
}
