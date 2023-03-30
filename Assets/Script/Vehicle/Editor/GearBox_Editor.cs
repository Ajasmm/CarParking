using Ajas.Vehicle;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GearBox))]
public class GearBox_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Find GearUp Speed"))
        {
            GearBox gearBox = (GearBox)serializedObject.targetObject;
            CalculateGearBoxData(gearBox);
        }
    }

    private void CalculateGearBoxData(GearBox gearBox)
    {
        if (gearBox == null) Debug.LogError("No gearBoxObject");
        if (gearBox.finalratio <= 0)
        {
            Debug.LogWarning($"Setting final drive from {gearBox.finalratio} To 1");
            gearBox.finalratio = 1;
        }

        float tyrePerimeter = gearBox.tyreRadius * 2 * Mathf.PI;
        float minSpeed, maxSpeed;
        foreach(Ratio ratio in gearBox.ratios)
        {
            minSpeed = (gearBox.gearDownRPM / ratio.ratio / gearBox.finalratio) * tyrePerimeter;
            minSpeed *= 0.06F;
            maxSpeed = (gearBox.gearUpRPM / ratio.ratio / gearBox.finalratio) * tyrePerimeter;
            maxSpeed *= 0.06F;

            ratio.gearDownSpeed = minSpeed;
            ratio.gearUpSpeed = maxSpeed;
        }
    }
}
