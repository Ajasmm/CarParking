using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DeleteLongDistanceItems : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Running");
        Transform[] AllTransforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
        Debug.Log($"A total of {AllTransforms.Length} found");
        Vector3 distance = Vector3.zero;

        foreach (Transform t in AllTransforms)
        {
            if (t.position.magnitude > 5000)
            {
                Debug.Log(t.name);
                DestroyImmediate(t.gameObject);
            }
        }

        Debug.Log("Execution completed");
    }
}
