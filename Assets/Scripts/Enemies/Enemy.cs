using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    public int last_swing_id = -1;

    SpriteRenderer sprite_renderer;
    public Sprite normal_sprite;
    public Sprite flash_sprite;

    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    public void SwordHit(int swing_id)
    {
        last_swing_id = swing_id;
        Hit();
    }

    public void Hit()
    {
        CancelInvoke();
        sprite_renderer.sprite = flash_sprite;
        Invoke("EndFlash", 0.1f);
    }

    void EndFlash()
    {
        sprite_renderer.sprite = normal_sprite;
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
        // Running into player
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (!player.invincible)
            {
                player.DamagePlayer(transform);
            }
        }
    }
}
