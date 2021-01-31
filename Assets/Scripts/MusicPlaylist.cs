using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioClip[] myMusic;
    AudioClip[] playedMusic;

    bool playerWantsMusic = true;
    bool loopActivated = false;
    bool musicStopped = false;

    public float fadeDuration = 1.0f;
    public float fadeTime;
    float mainVolume;
    int currentSong = 0;

    void reshuffle(AudioClip[] myMusic)
    {
        for (int t = 0; t < myMusic.Length; t++)
        {
            AudioClip tmp = myMusic[t];
            int r = Random.Range(t, myMusic.Length);
            myMusic[t] = myMusic[r];
            myMusic[r] = tmp;
        }
    }

    public string GetNameOfCurrentSong ()
    {
        string currentSongName = GetComponent<AudioSource>().clip.name;

        return currentSongName;
    }

    // Start is called before the first frame update
    void Start()
    {
        reshuffle(myMusic);

        //start with first music in the list
        GetComponent<AudioSource>().clip = myMusic[0];
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();

        mainVolume = GetComponent<AudioSource>().volume;

        GetNameOfCurrentSong();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying && playerWantsMusic == true)
        {
            currentSong++;

            if (currentSong >= myMusic.Length)
            {
                currentSong = 0;
            }

            GetComponent<AudioSource>().clip = myMusic[currentSong];
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();
        }
        
        //select next music in playlist
        if (GetComponent<AudioSource>().isPlaying && Input.GetKeyDown(KeyCode.N))
        {
            GetComponent<AudioSource>().Stop();
            currentSong++;

            if (currentSong >= myMusic.Length)
            {
                currentSong = 0;
            }

            GetComponent<AudioSource>().clip = myMusic[currentSong];
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();

            GetNameOfCurrentSong();
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

                musicStopped = true;
            }
                    
        }

        if (musicStopped && Input.GetKeyDown(KeyCode.B))
        {
            currentSong++;

            if (currentSong >= myMusic.Length)
            {
                currentSong = 0;
            }

            GetComponent<AudioSource>().clip = myMusic[currentSong];
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();

            musicStopped = false;

            GetNameOfCurrentSong();
        }

        if (!musicStopped && GetComponent<AudioSource>().volume < mainVolume)
        {
            GetComponent<AudioSource>().volume += Time.deltaTime; //fade in de la musique

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
