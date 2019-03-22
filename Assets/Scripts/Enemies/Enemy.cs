﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    CameraFollow camera_shake;

    public int health;
    public Dimension enemy_dimension;

    [HideInInspector]
    public int last_swing_id = -1;

    [HideInInspector]
    public bool knockback_state = false;
    float knockback_force = 500f;
    float knockback_time = 0.05f;

    SpriteRenderer sprite_renderer;
    public Sprite normal_sprite;
    public Sprite flash_sprite;
    public Sprite dead_sprite = null;

    Player player;
    public LayerMask block_mask;


    void Awake()
    {
        camera_shake = Camera.main.GetComponent<CameraFollow>();
        sprite_renderer = GetComponent<SpriteRenderer>();

        player = FindObjectOfType<Player>();
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
                
                foreach (Transform child in transform)
                {
                    child.GetComponent<SpriteRenderer>().color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 0.3f);
                    foreach (Collider2D collider in child.GetComponents<Collider2D>())
                    {
                        collider.enabled = false;
                    }
                }
            }
            else
            {
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);
                foreach (Collider2D collider in GetComponents<Collider2D>())
                {
                    collider.enabled = true;
                }

                foreach (Transform child in transform)
                {
                    child.GetComponent<SpriteRenderer>().color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);
                    foreach (Collider2D collider in child.GetComponents<Collider2D>())
                    {
                        collider.enabled = true;
                    }
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

            if (dead_sprite != null)
            {
                sprite_renderer.sprite = dead_sprite;
            }

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            Knockback(FindObjectOfType<Player>().transform);
            camera_shake.Shake(0.2f);

            if (GetComponent<ParticleSystem>() != null)
            {
                GetComponent<ParticleSystem>().Play();
            }
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

        if (health <= 0)
        {
            foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
            {
                collider.enabled = false;
            }
        }
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

    public bool CanSeePlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 400, block_mask);
        if (hit.collider != null)
        {
            if (hit.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
