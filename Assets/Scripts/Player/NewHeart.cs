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
            SetSpacing();

            Destroy(gameObject);
        }
    }

    void SetSpacing()
    {
        switch (heart_bar.transform.childCount)
        {
            case 3:
                heart_bar.GetComponent<VerticalLayoutGroup>().spacing = -55;
                break;
            case 4:
                heart_bar.GetComponent<VerticalLayoutGroup>().spacing = -40;
                break;
            case 5:
                heart_bar.GetComponent<VerticalLayoutGroup>().spacing = -25;
                break;
            case 6:
                heart_bar.GetComponent<VerticalLayoutGroup>().spacing = -10;
                break;
            case 7:
                heart_bar.GetComponent<VerticalLayoutGroup>().spacing = -1;
                break;
            case 8:
                heart_bar.GetComponent<VerticalLayoutGroup>().spacing = -1;
                break;
            case 9:
                heart_bar.GetComponent<VerticalLayoutGroup>().spacing = -1;
                break;
        }
    }
}
