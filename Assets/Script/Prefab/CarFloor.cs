using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFloor : MonoBehaviour
{
    [SerializeField] private float speed = 5F;
    Transform m_Transform;


    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
