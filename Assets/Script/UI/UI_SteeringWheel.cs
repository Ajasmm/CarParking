using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class UI_SteeringWheel : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [InputControl(layout = "Axis")]
    [SerializeField]
    private string m_ControlPath;

    [SerializeField] float maxRotation = 540;
    [SerializeField] float returnSpeed = 540;

    bool isPointerDown = false;
    Vector2 prevPos, currentPos;

    float deltaAngle, angle;
    RectTransform myTransform;

    Vector3 rotation = Vector3.zero;

    float value = 0;
    public float Value
    {
        get => value;
        private set => this.value = value;
    }

    protected override string controlPathInternal { 
        get => m_ControlPath; 
        set => m_ControlPath = value; 
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        myTransform = GetComponent<RectTransform>();

        isPointerDown = false;
        deltaAngle = 0;
        angle = 0;
        value = 0;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    private void Update()
    {
        if (!isPointerDown)
        {
            angle = Mathf.MoveTowards(angle, 0, returnSpeed * Time.deltaTime);
            if (Mathf.Abs(angle) < 1) angle = 0;
        }

        rotation.z = -angle;
        myTransform.localEulerAngles = rotation;

        value = angle / maxRotation;
        SendValueToControl(value);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentPos = eventData.position - (Vector2)myTransform.position;
        deltaAngle = -Vector2.SignedAngle(prevPos, currentPos);
        prevPos = currentPos;

        angle += deltaAngle;

        angle = Mathf.Clamp(angle, -maxRotation, maxRotation);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        prevPos = eventData.position - (Vector2)myTransform.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public float GetValue()
    {
        return value;
    }
}
