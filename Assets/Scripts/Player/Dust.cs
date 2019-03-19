using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public Sprite dust_1;
    public Sprite dust_2;
    public Sprite dust_3;

    int frame_count = 0;

    SpriteRenderer sprite_renderer;

    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        frame_count++;
        if (frame_count > 46)
        {
            Destroy(gameObject);
        }
        else if (frame_count > 38)
        {
            sprite_renderer.sprite = dust_3;
        }
        else if (frame_count > 30)
        {
            sprite_renderer.sprite = dust_2;
        }
    }
}
