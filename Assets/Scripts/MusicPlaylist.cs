using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioClip[] myMusic;
    AudioClip[] playedMusic;

    bool playerWantsMusic = true;
    bool loopActivated = false;

    public float fadeDuration = 1.0f;
    public float fadeTime;
    float volume = 1;
    float mainVolume;

    // Start is called before the first frame update
    void Start()
    {
        if(!GetComponent<AudioSource>().playOnAwake)
        {
            GetComponent<AudioSource>().clip = myMusic[Random.Range(0, myMusic.Length)];
            
            GetComponent<AudioSource>().loop = true;
            
            GetComponent<AudioSource>().Play();

        }

        mainVolume = GetComponent<AudioSource>().volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying && playerWantsMusic == true)
        {
            GetComponent<AudioSource>().clip = myMusic[Random.Range(0, myMusic.Length)];

            GetComponent<AudioSource>().loop = true;
            
            GetComponent<AudioSource>().Play();

        }
        
        //select next music in playlist
        if (GetComponent<AudioSource>().isPlaying && Input.GetKeyDown(KeyCode.N))
        {
            GetComponent<AudioSource>().Stop();
        }

        //stop radio
        if (GetComponent<AudioSource>().isPlaying && Input.GetKeyDown(KeyCode.B))
        {
            playerWantsMusic = false;
            fadeTime = fadeDuration;
        }

        if (playerWantsMusic == false && fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            GetComponent<AudioSource>().volume = fadeTime / fadeDuration;

            if (fadeTime <= 0)
            {
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().volume = 0;
            }
                    
        }

        if (playerWantsMusic)
        {

            if (GetComponent<AudioSource>().volume <= 0 && GetComponent<AudioSource>().volume < mainVolume)
            {
                GetComponent<AudioSource>().volume += Time.deltaTime;
            }

        }



        //restart radio
        if (!GetComponent<AudioSource>().isPlaying && Input.GetKeyDown(KeyCode.V) && (playerWantsMusic == false))
        {
            playerWantsMusic = true;
        }
            
        //active loop music
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (loopActivated == false)
            {
                GetComponent<AudioSource>().loop = true;
                loopActivated = true;
                Debug.Log("on a activé la loop");
            } 
            else
            {
                GetComponent<AudioSource>().loop = false;
                loopActivated = false;
                Debug.Log("on a désactivé la loop");
            }
            
        }

    }


}
