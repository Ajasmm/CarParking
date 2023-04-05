using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ajas.FrameWork;

public class ParkingLot : MonoBehaviour
{
    [SerializeField] private Transform centerTransform;
    [SerializeField] private float stayTime = 3F;
    [SerializeField] private Material hologramMaterial;

    private bool isPlayerIn = false;
    float tempTime;

    Transform playerTransform;
    Transform m_Transform;

    private void Awake()
    {
        isPlayerIn = false;
        tempTime = stayTime;
        hologramMaterial.SetFloat("_CustomAlpha", tempTime / stayTime);
        m_Transform = centerTransform;

    }
    private void Update()
    {
        if (!isPlayerIn || !GameManager.Instance.CurrentGamePlayMode.isPlaying) return;

        float distance, direction;
        Vector3 playerPos, myPos;
        playerPos = playerTransform.position;
        playerPos.y = 0;
        myPos = m_Transform.position;
        myPos.y = 0;
        distance = (playerPos - myPos).magnitude;

        direction = Vector3.Dot(playerTransform.forward, m_Transform.forward);


        if (distance > 0.5F) tempTime = stayTime;
        else if(Mathf.Abs(direction) < 0.75F) tempTime = stayTime;
        else tempTime -= Time.deltaTime;


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
        {
            isPlayerIn = true;
            playerTransform = other.gameObject.transform.root;
        }
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
