using Ajas.Vehicle;
using Ajas.FrameWork;
using UnityEngine;
using TMPro;
using System.Collections;

public class UI_VehicleMeter : MonoBehaviour
{
    [SerializeField] TMP_Text m_GearText, m_SpeedText, m_RPMText;

    private string gear;
    private int speed, rpm;

    Vehicle playerVehicle;


    private void Start()
    {
       StartCoroutine(GetPlayer());
    }
    private IEnumerator GetPlayer()
    {
        yield return GameManager.Instance.WaitForPlayerEnumerator();
        playerVehicle = GameManager.Instance.player.GetComponent<Vehicle>();
    }

    private void Update()
    {
        if (playerVehicle == null) return;

        speed = playerVehicle.GetSpeed();   
        rpm = playerVehicle.GetEngineRPM();
        gear = playerVehicle.GetCurrentGear();  

        m_SpeedText.text = "Speed: " + speed.ToString();
        m_RPMText.text = "RPM : " + rpm.ToString();
        m_GearText.text = gear.ToString();
    }
}
