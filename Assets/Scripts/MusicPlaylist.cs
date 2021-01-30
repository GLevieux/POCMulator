using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioClip[] myMusic;
    AudioClip[] playedMusic;
    bool playerWantsMusic = true;

    // Start is called before the first frame update
    void Start()
    {
        if(!GetComponent<AudioSource>().playOnAwake)
        {
            GetComponent<AudioSource>().clip = myMusic[Random.Range(0, myMusic.Length)];
            GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying && playerWantsMusic == true)
        {
            GetComponent<AudioSource>().clip = myMusic[Random.Range(0, myMusic.Length)];
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
            
            if (playerWantsMusic == false)
            GetComponent<AudioSource>().Stop();
        }
        
        //restart radio
        if (!GetComponent<AudioSource>().isPlaying && Input.GetKeyDown(KeyCode.V) && (playerWantsMusic == false))
        playerWantsMusic = true;

    }
}
