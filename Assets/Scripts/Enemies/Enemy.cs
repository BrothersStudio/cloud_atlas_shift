using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    CameraFollow camera_shake;

    public int health;
    public Dimension enemy_dimension;

    public int last_swing_id = -1;

    public bool knockback_state = false;
    float knockback_force = 1000f;
    float knockback_time = 0.05f;

    SpriteRenderer sprite_renderer;
    public Sprite normal_sprite;
    public Sprite flash_sprite;
    public Sprite dead_sprite;

    void Awake()
    {
        camera_shake = Camera.main.GetComponent<CameraFollow>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        TimeChange.current.enemies.Add(this);
        SwitchDimensions();
    }

    public void SwitchDimensions()
    {
        if (health > 0)
        {
            if (TimeChange.current.dimension != enemy_dimension)
            {
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 0.3f);
                foreach (Collider2D collider in GetComponents<Collider2D>())
                {
                    collider.enabled = false;
                }
            }
            else
            {
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);
                foreach (Collider2D collider in GetComponents<Collider2D>())
                {
                    collider.enabled = true;
                }
            }
        }
    }

    public void SwordHit(int damage, Transform source, int swing_id)
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
        if (health <= 0)
        {
            CancelInvoke();
            foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
            {
                collider.enabled = false;
            }

            sprite_renderer.sprite = dead_sprite;

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            Knockback(FindObjectOfType<Player>().transform);
            camera_shake.Shake(0.2f);
            GetComponent<ParticleSystem>().Play();
        }
    }

    void Knockback(Transform source)
    {
        Vector2 direction = source.position - transform.position;
        Vector2 norm_direction = direction.normalized;
        GetComponent<Rigidbody2D>().AddForce(-norm_direction * knockback_force);
        Invoke("EndKnockback", knockback_time);
        knockback_state = true;
    }

    void EndKnockback()
    {
        knockback_state = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Running into player
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (!player.invincible && TimeChange.current.dimension == enemy_dimension)
            {
                player.DamagePlayer(transform);
            }
        }
    }
}
