using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float flash_time = 0;
    float flash_speed = 0.2f;

    public bool invincible = false;
    float invincibility_end_time = 0f;
    float invincibility_time = 1f;

    SpriteRenderer sprite_renderer;
    public Sprite normal_sprite;
    public Sprite flash_sprite;

    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    public void Hit()
    {
        invincible = true;
        invincibility_end_time = Time.timeSinceLevelLoad + invincibility_time;

        flash_time = 0;
        sprite_renderer.sprite = flash_sprite;
    }

    void Update()
    {
        if (invincible)
        {
            flash_time += Time.deltaTime;
            if (flash_time > flash_speed)
            {
                FlipFlashSprite();
                flash_time = 0;
            }
        }

        if (Time.timeSinceLevelLoad > invincibility_end_time)
        {
            invincible = false;
            sprite_renderer.sprite = normal_sprite;
        }
    }

    void FlipFlashSprite()
    {
        if (sprite_renderer.sprite == normal_sprite)
        {
            sprite_renderer.sprite = flash_sprite;
        }
        else
        {
            sprite_renderer.sprite = normal_sprite;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            Debug.Log(player);
            if (!player.invincible)
            {
                player.DamagePlayer(transform);
            }
        }
    }
}
