using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public AudioClip heal_sound;
    GameObject heart_bar;

    void Awake()
    {
        heart_bar = GameObject.Find("Heart Bar");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (Transform child in heart_bar.transform)
            {
                if (!child.GetComponent<Heart>().full)
                {
                    heart_bar.GetComponent<AudioSource>().clip = heal_sound;
                    heart_bar.GetComponent<AudioSource>().Play();

                    collision.GetComponent<Player>().health++;

                    child.GetComponent<Heart>().FlipHeartSprite();
                    break;
                }
            }

            Destroy(gameObject);
        }
    }
}
