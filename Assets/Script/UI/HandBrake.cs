using Ajas.FrameWork;
using Ajas.Vehicle;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandBrake : MonoBehaviour
{
    [SerializeField] Image handBrake_Img;

    [SerializeField] Sprite handBrake_ON;
    [SerializeField] Sprite handBrake_OFF;

    bool handBrake;
    MyInput input;

    private void OnEnable()
    {
        input = GameManager.Instance.input;

        input.GamePlay.HandBrake.performed += OnHandBrake;
    }
    private void OnDisable()
    {
        input.GamePlay.HandBrake.performed -= OnHandBrake;
    }
    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        GameManager gameManager = GameManager.Instance;
        while (gameManager.player == null) { yield return null; }
        
        SetHandBrake(gameManager.player.GetComponent<Vehicle>().GetHandBrake());
    }
    private void SetHandBrake(bool state)
    {
        handBrake = state;
        if (state) handBrake_Img.sprite = handBrake_ON;
        else handBrake_Img.sprite = handBrake_OFF;
    }
    private void OnHandBrake(InputAction.CallbackContext context) { SetHandBrake(!handBrake); }

}
