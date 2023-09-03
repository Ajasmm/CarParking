using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource), typeof(Button))]
public class ButtonAudio : MonoBehaviour
{
    AudioSource buttonSound;
    Button button;


    private void Awake()
    {
        buttonSound = GetComponent<AudioSource>();
        button = GetComponent<Button>();

        if (button)
            button.onClick.AddListener(PlayAudio);
    }

    private void PlayAudio()
    {
        buttonSound.Stop();
        buttonSound.Play();
    }

    private void OnDisable()
    {
        if (button) button.onClick.RemoveListener(PlayAudio);
    }

}
