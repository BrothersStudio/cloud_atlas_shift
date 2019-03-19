using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    float opening_delay = 0.5f;
    public List<Sprite> sprites;

    public AudioClip start_open;
    public AudioClip end_open;

    public void Open()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[1];
        Invoke("FullyOpen", opening_delay);
        Camera.main.GetComponent<CameraFollow>().Shake(0.2f);

        GetComponent<AudioSource>().clip = start_open;
        GetComponent<AudioSource>().Play();
    }

    void FullyOpen()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[2];
        GetComponent<BoxCollider2D>().enabled = false;
        Camera.main.GetComponent<CameraFollow>().Shake(0.4f);

        GetComponent<AudioSource>().clip = end_open;
        GetComponent<AudioSource>().Play();
    }
}
