using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);   
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()    
    {
        PlayMusic("MainTheme");
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x=> x.soundName == name);

        if(sound == null)
        {
            Debug.Log("Sound not found!");
        }
        else 
        {
            musicSource.clip = sound.clip;
            musicSource.Play(); 
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x=> x.soundName == name);

        if(sound == null)
        {
            Debug.Log("Sound not found!");
        }
        else 
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
