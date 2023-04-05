using Ajas.FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.input.Menu.Escape.performed += Context => OnEscape();
    }
    private void OnDisable()
    {
        GameManager.Instance.input.Menu.Escape.performed -= Context => OnEscape();
    }
    private void OnEscape()
    {
        gameObject.SetActive(false);
    }
}
