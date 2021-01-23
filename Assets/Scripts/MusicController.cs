using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] songs;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if(!source.isPlaying)
        {
            source.clip = songs[Random.Range(0, songs.Length - 1)];
            source.Play();
        }
    }

}
