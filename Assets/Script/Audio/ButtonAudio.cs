using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour
{
    [SerializeField] AudioMixerGroup audioMixerGroup;
    [SerializeField] AudioClip audioClip;
    
    AudioSource buttonSound;
    Button button;


    private void Awake()
    {
        button = GetComponent<Button>();

        if(button) button.onClick.AddListener(PlayAudio);
    }

    private void PlayAudio()
    {
        GameObject newObj = new GameObject("Button sound Emitter");
        DontDestroyOnLoad(newObj);

        newObj.AddComponent<Killer>();  
        
        buttonSound = newObj.AddComponent<AudioSource>();
        buttonSound.playOnAwake = false;
        buttonSound.Stop();
        buttonSound.clip = audioClip;
        buttonSound.outputAudioMixerGroup = audioMixerGroup;
        buttonSound.Play();
    }

    private void OnDisable()
    {
        if (button) button.onClick.RemoveListener(PlayAudio);
    }

}
