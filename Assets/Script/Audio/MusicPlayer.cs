using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] float transitionTime = 2;
    [SerializeField] float maxVolume = 0.5F;
    
    AudioSource musicSource;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.playOnAwake = true;
        musicSource.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float musicVolume = musicSource.volume;

        musicVolume += ( 1 / transitionTime) * Time.deltaTime;
        if (musicVolume > maxVolume)
        {
            musicVolume = maxVolume;
            musicSource.volume = musicVolume;
            this.enabled = false;
            return;
        }

        musicSource.volume = musicVolume;
    }
}
