using Ajas.FrameWork;
using UnityEngine;

public class AndroidController : MonoBehaviour
{
    [SerializeField] GameObject Button_Controller;
    [SerializeField] GameObject Wheel_Controller;

    private void OnEnable()
    {
        // To load data we need to Initialize the GameManager
        GameManager.Instance.Initialize();
        DisableAllControlls();

        Controls.SteeringControlMode controlMode;
        controlMode = (Controls.SteeringControlMode)PlayerPrefs.GetInt(Controls.CONTROLLER_MODE);

        switch(controlMode)
        {
            case Controls.SteeringControlMode.Wheel:
                Wheel_Controller.SetActive(true); 
                break;
            case Controls.SteeringControlMode.Button:
                Button_Controller.SetActive(true);
                break;
        }

        if (Application.platform != RuntimePlatform.Android)
        {
            DisableAllControlls();
        }
    }

    private void DisableAllControlls()
    {
        Button_Controller.SetActive(false);
        Wheel_Controller.SetActive(false);
    }
}
