using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvionSounds : MonoBehaviour
{
    enum EtatAcceleration
    {
        STOPPED,
        STARTING,
        RUNNING
    };

    private EtatAcceleration etatAcceleration;

    private MusicPlaylist.EtatPlaylist etatPlaylist;

    public AudioSource audioSourceStart;
    public AudioSource audioSourceRun;
    public AudioSource audioSourceLift;
    public AudioSource audioSourceEngineSlow;

    public float fadeDuration = 1.0f;
    float fadeTime;
    float mainVolume;

    // Start is called before the first frame update
    void Start()
    {
        mainVolume = GetComponent<AudioSource>().volume;
        fadeTime = fadeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (etatAcceleration == EtatAcceleration.STOPPED && MusicPlaylist.etatPlaylist == MusicPlaylist.EtatPlaylist.STOPPED)
        {  
            if (audioSourceEngineSlow.isPlaying == false)
            {
                audioSourceEngineSlow.volume += Time.deltaTime;

                if (GetComponent<AudioSource>().volume == 1)
                {
                    audioSourceEngineSlow.Play();
                    audioSourceEngineSlow.loop = true;
                }
            }

        } else if (etatAcceleration == EtatAcceleration.STOPPED && MusicPlaylist.etatPlaylist == MusicPlaylist.EtatPlaylist.PLAYING)
        {
            if (audioSourceEngineSlow.isPlaying == true)
            {
                fadeTime -= Time.deltaTime;
                audioSourceEngineSlow.volume = fadeTime / fadeDuration;

                if (fadeTime <= 0)
                {
                    audioSourceEngineSlow.Stop();
                    audioSourceEngineSlow.volume = mainVolume;
                    fadeTime = fadeDuration;
                }   
            }
        }
        
        if (etatAcceleration == EtatAcceleration.STARTING)
        {
            if (audioSourceEngineSlow.isPlaying == true)
            {
                fadeTime -= Time.deltaTime;
                audioSourceEngineSlow.volume = fadeTime / fadeDuration;

                if (fadeTime <= 0)
                {
                    audioSourceEngineSlow.Stop();
                    audioSourceEngineSlow.volume = mainVolume;
                    fadeTime = fadeDuration;
                }
            }

            if (audioSourceStart.isPlaying == false)
            {
                audioSourceRun.Play();
                audioSourceRun.loop = true;

                etatAcceleration = EtatAcceleration.RUNNING;
            }
        }

        if (Input.GetButton("LeftEngineThrusterIncrease") || Input.GetButton("RightEngineThrusterIncrease") || Input.GetButton("VerticalThrusterIncrease") || Input.GetButton("VerticalThrusterDecrease"))
        {
            if (audioSourceEngineSlow.isPlaying == true)
            {
                fadeTime -= Time.deltaTime;
                audioSourceEngineSlow.volume = fadeTime / fadeDuration;

                if (fadeTime <= 0)
                {
                    audioSourceEngineSlow.Stop();
                    audioSourceEngineSlow.volume = mainVolume;
                    fadeTime = fadeDuration;
                }
            }

            if (etatAcceleration == EtatAcceleration.STOPPED)
            {
                audioSourceStart.Play();
                audioSourceStart.loop = false;
                etatAcceleration = EtatAcceleration.STARTING;
            }
            

        } else
        {
            audioSourceRun.Stop();

            etatAcceleration = EtatAcceleration.STOPPED;
        }
        /*
        if (Input.GetButton("VerticalThrusterIncrease") || Input.GetButton("VerticalThrusterIncrease"))
        {
            if (etatAcceleration == EtatAcceleration.STOPPED)
            {
                audioSourceLift.Play();
                audioSourceLift.loop = false;
                etatAcceleration = EtatAcceleration.STARTING;
            }

        }*/
    }
}
