using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvionSounds : MonoBehaviour
{
    public AudioClip thrusterStart;
    public AudioClip thrusterRepeat;

    public float volumeThrusters = 1.0f;

    enum EtatAcceleration
    {
        STOPPED,
        STARTING,
        RUNNING
    };

    private EtatAcceleration etatAcceleration;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (etatAcceleration == EtatAcceleration.STARTING)
        {
            if (audioSource.isPlaying == false)
            {
                audioSource.clip = thrusterRepeat;
                audioSource.Play();
                audioSource.loop = true;

                etatAcceleration = EtatAcceleration.RUNNING;
            }
        }

        if (Input.GetButton("LeftEngineThrusterIncrease") || Input.GetButton("RightEngineThrusterIncrease"))
        {
            
            if (etatAcceleration == EtatAcceleration.STOPPED)
            {
                audioSource.PlayOneShot(thrusterStart, volumeThrusters);
                etatAcceleration = EtatAcceleration.STARTING;
            }
            

        } else
        {
            audioSource.Stop();

            etatAcceleration = EtatAcceleration.STOPPED;
        }
    }
}
