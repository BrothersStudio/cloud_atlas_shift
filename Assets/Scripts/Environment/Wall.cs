using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool is_block;

    public Sprite orange_sprite;
    public Sprite blue_sprite;
    SpriteRenderer sprite_renderer;

    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();

        TimeChange.current.walls.Add(this);

        CheckColor();
    }

    void CheckColor()
    {
        if (!is_block)
        {
            if (TimeChange.current.dimension == Dimension.Blue)
            {
                sprite_renderer.sprite = blue_sprite;
            }
            else
            {
                sprite_renderer.sprite = orange_sprite;
            }
        }
        else
        {
            GetComponent<BlockAnimation>().CheckColor();
        }
    }

    public void SwitchDimensions()
    {
        if (!is_block)
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
        else
        {
            GetComponent<BlockAnimation>().SwitchDimensions();
        }
    }
}
