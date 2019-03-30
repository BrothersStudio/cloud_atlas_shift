using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHeart : MonoBehaviour
{
    public AudioClip new_heart_sound;
    GameObject heart_bar;
    public GameObject heart_ui;

    void Awake()
    {
        heart_bar = GameObject.Find("Heart Bar");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (Transform child in heart_bar.transform)
            {
                if (!child.GetComponent<Heart>().full)
                {
                    child.GetComponent<Heart>().FlipHeartSprite();
                }
            }

            heart_bar.GetComponent<AudioSource>().clip = new_heart_sound;
            heart_bar.GetComponent<AudioSource>().Play();

            Instantiate(heart_ui, heart_bar.transform);
            FindObjectOfType<SetHearts>().SetSpacing();

            Destroy(gameObject);
        }
    }
}
