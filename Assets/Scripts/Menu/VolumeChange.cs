using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeChange : MonoBehaviour
{
    public AudioSource audioSrc;
    private float musicVolume = 0.5f;
    // Start is called before the first frame update
    public void Start()
    {
        // audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {
        // PlayerPrefs.SetInt("score", playerScore);
        // audioSrc.volume = musicVolume;
    }

    public void setVolume(float vol)
    {
        // musicVolume = vol;
    }
}
