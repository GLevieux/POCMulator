using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioClip[] myMusic;
    AudioClip[] playedMusic;
    enum EtatPlaylist
    {
        PLAYING,
        STARTING,
        STOPPING,
        STOPPED,
        CHANGING,
        CHANGED
    };

    private EtatPlaylist etatPlaylist;

    bool loopActivated = false;

    public float fadeDuration = 1.0f;
    float fadeTime;
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
        
        GetComponent<AudioSource>().clip = myMusic[0]; //start with first music in the list

        mainVolume = GetComponent<AudioSource>().volume;
        fadeTime = fadeDuration;

        GetComponent<AudioSource>().volume = 0; //useful for the opening fade in
        GetComponent<AudioSource>().loop = false;

        GetComponent<AudioSource>().Play();

        GetNameOfCurrentSong();

        etatPlaylist = EtatPlaylist.STARTING;
    }

    // Update is called once per frame
    void Update()
    {
        //music stopped, autoplay is activated by default
        if (etatPlaylist == EtatPlaylist.PLAYING && !GetComponent<AudioSource>().isPlaying)
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
        if (Input.GetKeyDown(KeyCode.N))
        {
            fadeTime = fadeDuration; 
            etatPlaylist = EtatPlaylist.CHANGING;
        }

        if (etatPlaylist == EtatPlaylist.CHANGING && fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            GetComponent<AudioSource>().volume = fadeTime / fadeDuration;

            if (fadeTime <= 0)
            {
                GetComponent<AudioSource>().Stop();

                currentSong++;

                if (currentSong >= myMusic.Length)
                {
                    currentSong = 0;
                }

                GetComponent<AudioSource>().clip = myMusic[currentSong];
                GetComponent<AudioSource>().loop = false;

                etatPlaylist = EtatPlaylist.CHANGED;
            }        
        }

        if (etatPlaylist == EtatPlaylist.CHANGED)
        {
            if (GetComponent<AudioSource>().volume >= 0)
            {
                GetComponent<AudioSource>().volume += Time.deltaTime;
            }

            if (GetComponent<AudioSource>().volume == 1)
            {
                GetComponent<AudioSource>().Play();

                GetNameOfCurrentSong();

                etatPlaylist = EtatPlaylist.PLAYING;
            } 
        }

        //stop playing music
        if (Input.GetKeyDown(KeyCode.B) && etatPlaylist == EtatPlaylist.PLAYING)
        {
            fadeTime = fadeDuration;
            etatPlaylist = EtatPlaylist.STOPPING;
        }

        //fade out music
        if (etatPlaylist == EtatPlaylist.STOPPING && fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            GetComponent<AudioSource>().volume = fadeTime / fadeDuration;

            if (fadeTime <= 0)
            {
                GetComponent<AudioSource>().Stop();

                etatPlaylist = EtatPlaylist.STOPPED;
            }
        }

        //restart playing music
        if (Input.GetKeyDown(KeyCode.B) && etatPlaylist == EtatPlaylist.STOPPED)
        {
            if (GetComponent<AudioSource>().volume == 0)
                GetComponent<AudioSource>().Play();

            etatPlaylist = EtatPlaylist.STARTING;
        }

        //fade in music
        if (etatPlaylist == EtatPlaylist.STARTING && GetComponent<AudioSource>().volume >= 0)
        {
            GetComponent<AudioSource>().volume += Time.deltaTime;

            if (GetComponent<AudioSource>().volume == 1)
                etatPlaylist = EtatPlaylist.PLAYING;   
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
