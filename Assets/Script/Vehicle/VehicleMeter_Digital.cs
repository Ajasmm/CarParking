using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VehicleMeter_Digital : MonoBehaviour
{
    [SerializeField] float redSpeed = 90F;
    [SerializeField] float redRPM = 7000F;

    [SerializeField] TMP_Text speed_Text;
    [SerializeField] TMP_Text rpm_Text;
    

    public void UpdateMeter(float speed, float rpm)
    {
        if(speed_Text)
        {
            speed_Text.text = ((int)speed).ToString();
            speed_Text.color = (speed > redSpeed) ? Color.red : Color.white;
        }

        if(rpm_Text)
        {
            rpm_Text.text = ((int)rpm).ToString();
            rpm_Text.color = (rpm > redRPM) ? Color.red : Color.white;
        }
    }
}
