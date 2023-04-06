using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] private TMP_Text m_Text;
    public Action OnButtonClick;

    private void Start()
    {
        button.onClick.AddListener(() => { if(OnButtonClick != null) OnButtonClick(); });
    }

    public void SetText(string text)
    {
        m_Text.text = text;
    }
    public void SetInteractionMode(bool value)
    {
        button.interactable = value;
    }
}
