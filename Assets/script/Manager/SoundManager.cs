using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGMSource;
    public AudioSource SFXSource;


    public AudioClip[] audioList;

    private void Start()
    {
        BGMSource.volume = 0.4f;
        SFXSource.volume = 0.4f;
    }

    public void BGMVolume(float volume)
    {
        BGMSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
    public void SoundChange()
    {
        //Top
        BGMSource.Stop();
        BGMSource.clip = audioList[2];
        BGMSource.Play();
    }
    public void SoundChange2()
    {
        //Town
        BGMSource.Stop();
        BGMSource.clip = audioList[0];
        BGMSource.Play();
    }
    public void SoundChange3()
    {
        //Boss
        BGMSource.Stop();
        BGMSource.clip = audioList[1];
        BGMSource.Play();
    }


    public void SoundChange4()
    {
        //Field
        BGMSource.Stop();
        BGMSource.clip = audioList[3];
        BGMSource.Play();
    }

}
