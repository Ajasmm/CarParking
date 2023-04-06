using Ajas.Vehicle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintChanger : MonoBehaviour
{
    [SerializeField] private int bodySubMeshIndex = 0;
    [SerializeField] private MeshRenderer meshRenderer;

    public void ChangePaint(Material material)
    {   
        Material[] materials = meshRenderer.materials;
        materials[bodySubMeshIndex] = material;
        meshRenderer.materials = materials;

        Vehicle vehicle = GetComponent<Vehicle>();
        vehicle.brakeMaterial = materials[1];
    }
}
