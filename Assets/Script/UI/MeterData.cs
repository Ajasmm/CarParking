using UnityEngine;

[CreateAssetMenu(menuName ="Vehicle/MeterData" , fileName ="Meter Data")]
public class MeterData : ScriptableObject
{
    public string gear;
    public int speed;
    public int rpm;
}