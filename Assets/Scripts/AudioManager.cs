using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public Sound[] musicSound, sfxSound;
    public AudioSource musicSource, sfxSource;
    
    public GameObject pauseMusicIcon;
    public GameObject pauseSfxIcon;

    private bool MusicIcon = false;
    private bool sfxIcon = false;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        PlayMusic("Theme");
    }
    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSound, x=> x.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            
        }
        else
        {
            musicSource.volume = 0.1f;
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound sound_sfx = Array.Find(sfxSound, x=> x.name == name);
        if (sound_sfx == null)
        {
            Debug.LogWarning("Sound_SFX: " + name + " not found!");
            
        }
        else
        {
            sfxSource.volume = (name == "Spawn") ? 0.05f : 0.3f;
            sfxSource.PlayOneShot(sound_sfx.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        if (MusicIcon == false)
        {
            pauseMusicIcon.SetActive(true);
            MusicIcon = true;
        }
        else
        {
            pauseMusicIcon.SetActive(false);
            MusicIcon = false;
        }
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void ToggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
        if (sfxIcon == false)
        {
            pauseSfxIcon.SetActive(true);
            sfxIcon = true;
        }
        else
        {
            pauseSfxIcon.SetActive(false);
            sfxIcon = false;
        }
    }
    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
