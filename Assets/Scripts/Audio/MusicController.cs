using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    bool minor_playing = true;
    public AudioSource minor;
    public AudioSource major;

    public void SwapMusic()
    {
        if (minor_playing)
        {
            minor_playing = false;
            minor.volume = 0;
            major.volume = 0.5f;
        }
        else
        {
            minor_playing = true;
            major.volume = 0;
            minor.volume = 0.5f;
        }
    }
}
