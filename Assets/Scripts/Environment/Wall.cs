using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite orange_sprite;
    public Sprite blue_sprite;
    SpriteRenderer sprite_renderer;

    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();

        TimeChange.current.walls.Add(this);
    }

    public void SwitchDimensions()
    {
        if (sprite_renderer.sprite == orange_sprite)
        {
            sprite_renderer.sprite = blue_sprite;
        }
        else
        {
            sprite_renderer.sprite = orange_sprite;
        }
    }
}
