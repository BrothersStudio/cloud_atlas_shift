using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    bool minor_playing = true;
    public AudioSource minor;
    public AudioSource major;

    public AudioSource boss_laugh_source;

    public AudioClip boss_minor;
    public AudioClip boss_major;

    bool victory = false;

    public void SetMinorMusic()
    {
        minor_playing = true;
        major.volume = 0;
        minor.volume = 0.5f;
    }

    public void SwapMusic()
    {
        if (!victory)
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

    public void PlayBossLaugh()
    {
        minor.Stop();
        major.Stop();

        boss_laugh_source.Play();
    }

    public void SetBossMusic()
    {
        boss_laugh_source.Stop();

        minor.clip = boss_minor;
        minor.Play();

        major.clip = boss_major;
        major.Play();
    }

    public void SetVictoryMusic()
    {
        victory = true;
    }
}
