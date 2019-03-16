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

    public void SwordHit(int damage, int swing_id)
    {
        last_swing_id = swing_id;
        Hit(damage);
    }

    public void Hit(int damage)
    {
        CancelInvoke();
        sprite_renderer.sprite = flash_sprite;
        Invoke("EndFlash", 0.1f);

        ParseDamage(damage);
    }

    void EndFlash()
    {
        sprite_renderer.sprite = normal_sprite;
    }

    void ParseDamage(int damage)
    {
        health -= damage;
    }

    void OnTriggerStay2D(Collider2D collision)
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
