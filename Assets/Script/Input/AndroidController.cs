using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidController : MonoBehaviour
{
    private void OnEnable()
    {
        if(Application.platform != RuntimePlatform.Android) this.gameObject.SetActive(false);
    }
}
