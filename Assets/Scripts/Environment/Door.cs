using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    float opening_delay = 0.5f;
    public List<Sprite> sprites;

    public void Open()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[1];
        Invoke("FullyOpen", opening_delay);
    }

    void FullyOpen()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[2];
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
