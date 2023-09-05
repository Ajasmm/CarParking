using UnityEngine;
using TMPro;

public class UI_VehicleMeter : MonoBehaviour
{
    [SerializeField] TMP_Text m_GearText, m_SpeedText, m_RPMText;

    [SerializeField] MeterData m_MeterData;

    private void Update()
    {
        if (m_MeterData == null) return;

        m_SpeedText.text = "Speed: " + m_MeterData.speed.ToString();
        m_RPMText.text = "RPM : " + m_MeterData.rpm.ToString();
        m_GearText.text = m_MeterData.gear.ToString();
    }
}
