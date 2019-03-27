using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip shot;
    public AudioClip sword;

    public AudioClip player_hurt;

    public AudioClip player_death;

    public AudioClip enemy_sword_death;
    public AudioClip enemy_gun_death;

    public void Shoot()
    {
        GetComponent<AudioSource>().clip = shot;
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
    }

    public void Slash()
    {
        GetComponent<AudioSource>().clip = sword;
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
    }

    public void Hurt()
    {
        GetComponent<AudioSource>().clip = player_hurt;
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
    }

    public void Dead()
    {
        GetComponent<AudioSource>().clip = player_death;
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
    }
}
