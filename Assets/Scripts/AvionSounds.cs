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

    public AudioSource audioSourceStart;
    public AudioSource audioSourceRun;
    public AudioSource audioSourceLift;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (etatAcceleration == EtatAcceleration.STARTING)
        {
            if (audioSourceStart.isPlaying == false)
            {
                audioSourceRun.Play();
                audioSourceRun.loop = true;

                etatAcceleration = EtatAcceleration.RUNNING;
            }

        }

        if (Input.GetButton("LeftEngineThrusterIncrease") || Input.GetButton("RightEngineThrusterIncrease") || Input.GetButton("VerticalThrusterIncrease") || Input.GetButton("VerticalThrusterDecrease"))
        {
            
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
