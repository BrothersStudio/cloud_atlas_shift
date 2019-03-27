using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    Enemy enemy;
    SpriteRenderer sprite_renderer;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ChooseSprite();
    }

    void ChooseSprite()
    {
        if (enemy.flashing)
        {
            sprite_renderer.sprite = enemy.flash_sprite;
        }
        else
        {
            sprite_renderer.sprite = enemy.normal_sprite;
        }
    }
}
