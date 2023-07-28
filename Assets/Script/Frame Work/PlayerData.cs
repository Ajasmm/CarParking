[System.Serializable]
public struct PlayerData
{
    public int highestLevelReached;
    public string vehicleName;
    public VehicleColor vehicleColor;

    public PlayerData(int defaultHighestLevel)
    {
        this.highestLevelReached = defaultHighestLevel;
        vehicleName = "Car_1_Player";
        vehicleColor = VehicleColor.Blue;
    }
    public enum VehicleColor
    {
        Blue,
        Purple,
        Red,
        Green,
        Yellow,
        Silver
    }
}