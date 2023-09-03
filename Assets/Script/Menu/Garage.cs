using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ajas.FrameWork;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Garage : MonoBehaviour
{
    [SerializeField] Button next;
    [SerializeField] Button prev;

    [SerializeField] Button blue_Color;
    [SerializeField] Button purple_Color;
    [SerializeField] Button red_Color;
    [SerializeField] Button green_Color;
    [SerializeField] Button yellow_Color;
    [SerializeField] Button silver_Color;

    [SerializeField] List<string> vehicleNames;

    int currentVehicleIndex = 0;

    MyInput input;

    private void OnEnable()
    {
        input = GameManager.Instance.input;
        input.Menu.Escape.performed += OnEscape;

        currentVehicleIndex = vehicleNames.IndexOf(GameManager.Instance.PlayerData.vehicleName);
        next.onClick.AddListener(OnNext);
        prev.onClick.AddListener(OnPrev);

        blue_Color.onClick.AddListener(() => OnChangeMaterial(PlayerData.VehicleColor.Blue));
        purple_Color.onClick.AddListener(() => OnChangeMaterial(PlayerData.VehicleColor.Purple));
        red_Color.onClick.AddListener(() => OnChangeMaterial(PlayerData.VehicleColor.Red));
        green_Color.onClick.AddListener(() => OnChangeMaterial(PlayerData.VehicleColor.Green));
        yellow_Color.onClick.AddListener(() => OnChangeMaterial(PlayerData.VehicleColor.Yellow));
        silver_Color.onClick.AddListener(() => OnChangeMaterial(PlayerData.VehicleColor.Silver));
    }
    private void OnDisable()
    {
        input.Menu.Escape.performed -= OnEscape;

        next.onClick.RemoveListener(OnNext);
        prev.onClick.RemoveListener(OnPrev);

        blue_Color.onClick.RemoveAllListeners();
        purple_Color.onClick.RemoveAllListeners();
        red_Color.onClick.RemoveAllListeners();
        green_Color.onClick.RemoveAllListeners();
        yellow_Color.onClick.RemoveAllListeners();
        silver_Color.onClick.RemoveAllListeners();
    }
    private void OnNext()
    {
        currentVehicleIndex++;
        if (currentVehicleIndex >= vehicleNames.Count)
            currentVehicleIndex = 0;

        GameManager.Instance.UpdatePlayer(vehicleNames[currentVehicleIndex]);
    }
    private void OnPrev()
    {
        currentVehicleIndex--;
        if (currentVehicleIndex < 0)
            currentVehicleIndex = vehicleNames.Count - 1;

        GameManager.Instance.UpdatePlayer(vehicleNames[currentVehicleIndex]);
    }
    private void OnEscape(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }

    
    private void OnChangeMaterial(PlayerData.VehicleColor color)
    {
        GameManager.Instance.ChangePlayerMaterial(color);

    }
}
