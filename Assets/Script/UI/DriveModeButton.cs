using Ajas.FrameWork;
using Ajas.Vehicle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DriveModeButton : MonoBehaviour
{
    [SerializeField] Image drive_img;
    [SerializeField] Image nuteral_img;
    [SerializeField] Image reverce_img;

    [SerializeField] Sprite driveON, driveOFF;
    [SerializeField] Sprite nuteralON, nuteralOFF;
    [SerializeField] Sprite reverceON, reverceOFF;

    MyInput input;

    private void OnEnable()
    {
        input = GameManager.Instance.input;

        input.GamePlay.Drive.performed += Drive;
        input.GamePlay.Nuteral.performed += Nuteral;
        input.GamePlay.Reverce.performed += Reverce;
    }
    private void OnDisable()
    {
        input.GamePlay.Drive.performed -= Drive;
        input.GamePlay.Nuteral.performed -= Nuteral;
        input.GamePlay.Reverce.performed -= Reverce;
    }
    void Start()
    {
        StartCoroutine(Initialize());
    }

    private void Drive(InputAction.CallbackContext context) { SetToggle(DriveMode.DRIVE); }
    private void Nuteral(InputAction.CallbackContext context) { SetToggle(DriveMode.NUTERAL); }
    private void Reverce(InputAction.CallbackContext context) { SetToggle(DriveMode.REVERCE); }

    private void SetToggle(DriveMode currentDriveMode)
    {
        DisableAllButton();
        switch (currentDriveMode)
        {
            case DriveMode.DRIVE:
                drive_img.sprite = driveON;
                break;
            case DriveMode.NUTERAL:
                nuteral_img.sprite = nuteralON;
                break;
            case DriveMode.REVERCE:
                reverce_img.sprite = reverceON;
                break;
        }
    }
    private void DisableAllButton()
    {
        drive_img.sprite = driveOFF;
        nuteral_img.sprite = nuteralOFF;
        reverce_img.sprite = reverceOFF;
    }

    IEnumerator Initialize()
    {
        GameManager gameManager = GameManager.Instance;
        while(gameManager.player == null)
        {
            yield return null;
        }

        Vehicle vehicle = gameManager.player.GetComponent<Vehicle>();
        DriveMode currentDriveMode = vehicle.GetCurrentDriveMode();

        SetToggle(currentDriveMode);
    }

    
}
