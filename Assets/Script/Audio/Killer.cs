using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Kill", 2F);   
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
