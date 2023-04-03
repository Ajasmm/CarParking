using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ajas.FrameWork;

public class ParkingLot : MonoBehaviour
{
    [SerializeField] private float stayTime = 3F;
    [SerializeField] private Material hologramMaterial;

    private bool isPlayerIn = false;
    float tempTime;

    private void Awake()
    {
        tempTime = stayTime;
        hologramMaterial.SetFloat("_CustomAlpha", tempTime / stayTime);

    }
    private void Update()
    {
        if (!isPlayerIn || !GameManager.Instance.CurrentGamePlayMode.isPlaying) return;

        tempTime -= Time.deltaTime;
        hologramMaterial.SetFloat("_CustomAlpha", tempTime / stayTime);
        if (tempTime < 0)
        {
            tempTime = 0;
            GameManager.Instance.GameWon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            isPlayerIn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerIn = false;
            tempTime = stayTime;
            hologramMaterial.SetFloat("_CustomAlpha", tempTime / stayTime);
        }
    }

    public void Reset()
    {
        Awake();
    }
}
