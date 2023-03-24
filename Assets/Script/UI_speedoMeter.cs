using Ajas.Vehicle;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_speedoMeter : MonoBehaviour
{
    [SerializeField] TMP_Text speed;
    [SerializeField] TMP_Text rpm;
    [SerializeField] TMP_Text currentGear;

    Vehicle playerVehicle;
    CancellationTokenSource cancellationTokenSource;

    bool isInitialised = false;

    private void OnEnable()
    {
        GetPlayer();
    }

    private void Update()
    {
        if (!isInitialised) return;
        if(speed) speed.text = playerVehicle.GetSpeed().ToString();
        if(rpm) rpm.text = playerVehicle.GetEngineRPM().ToString();
        if(currentGear) currentGear.text = playerVehicle.GetCurrentGear().ToString();

    }

    async void GetPlayer()
    {
        if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource();

        await Task.Run(() =>
        {
            Debug.Log("GameManagere");
            while(GameManager.Instance == null)
            {
                if (cancellationTokenSource.IsCancellationRequested) return;
                Debug.Log("Gamemanager is null");
            }
            while (GameManager.Instance.player == null)
            {
                if(cancellationTokenSource.IsCancellationRequested) return;
                Debug.Log("Player not registered");
            }
        }, cancellationTokenSource.Token); 
        
        playerVehicle = GameManager.Instance.player.GetComponent<Vehicle>();
        isInitialised = true;
    }
    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }
}
