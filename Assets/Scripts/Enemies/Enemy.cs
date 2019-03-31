using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public CameraFollow camera_shake;

    public bool is_boss;
    public bool is_hazard;

    public int health;
    public Dimension enemy_dimension;

    [HideInInspector]
    public int last_swing_id = -1;
    DamageType last_damage_type;

    [HideInInspector]
    public bool knockback_state = false;
    float knockback_force = 500f;

    [HideInInspector]
    public bool flashing = false;

    bool fading = false;
    float start_fade_time = 1;
    float fade_rate = 1000 / 255f;

    SpriteRenderer sprite_renderer;
    public Sprite normal_sprite;
    public Sprite flash_sprite;
    public Sprite dead_sprite = null;

    Player player;
    public LayerMask block_mask;

    [HideInInspector]
    public Vector3 max_pos;
    [HideInInspector]
    public Vector3 min_pos;

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

    void Update()
    {
        if (fading)
        {
            Color current_color = sprite_renderer.color;
            current_color.a -= fade_rate * Time.deltaTime;
            sprite_renderer.color = current_color;

            if (current_color.a <= 0)
            {
                Destroy(gameObject);
            }
        } 
        else if (IsOutOfBounds() && !is_hazard)
        {
            EmergencyKill();
        }
    }

    public void SwitchDimensions()
    {
        if (health > 0 && !is_boss)
        {
            Collider2D player_collider = player.GetComponent<Collider2D>();
            if (TimeChange.current.dimension != enemy_dimension)
            {
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 0.3f);
                foreach (Collider2D collider in GetComponents<Collider2D>())
                {
                    Physics2D.IgnoreCollision(player_collider, collider, true);
                }
                
                foreach (Transform child in transform)
                {
                    child.GetComponent<SpriteRenderer>().color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 0.3f);
                    foreach (Collider2D collider in child.GetComponents<Collider2D>())
                    {
                        Physics2D.IgnoreCollision(player_collider, collider, true);
                    }
                }
            }
            else
            {
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);
                foreach (Collider2D collider in GetComponents<Collider2D>())
                {
                    Physics2D.IgnoreCollision(player_collider, collider, false);
                }

                foreach (Transform child in transform)
                {
                    child.GetComponent<SpriteRenderer>().color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);
                    foreach (Collider2D collider in child.GetComponents<Collider2D>())
                    {
                        Physics2D.IgnoreCollision(player_collider, collider, false);
                    }
                }
            }
        }
    }

    public void SwordHit(int damage, Transform source, int swing_id)
    {
        last_swing_id = swing_id;
        last_damage_type = DamageType.Sword;

        CancelInvoke();
        sprite_renderer.sprite = flash_sprite;

        flashing = true;
        Invoke("EndFlash", 0.1f);

        Knockback(player.transform);
        ParseDamage(damage);
    }

    public void GunHit(int damage)
    {
        last_damage_type = DamageType.Gun;

        CancelInvoke();
        sprite_renderer.sprite = flash_sprite;

        flashing = true;
        Invoke("EndFlash", 0.1f);

        ParseDamage(damage);
    }

    void EndFlash()
    {
        flashing = false;
    }

    void ParseDamage(int damage)
    {
        PlayHitSound();

        health -= damage;
        if (health <= 0 && !is_boss)
        {
            Die();
        }
    }

    void PlayHitSound()
    {
        AudioSource source = GetComponent<AudioSource>();
        if (!source.isPlaying)
        {
            AudioClip chosen_clip = last_damage_type == DamageType.Sword ? player.GetComponent<PlayerAudio>().enemy_sword_death : player.GetComponent<PlayerAudio>().enemy_gun_death;
            source.clip = chosen_clip;
            source.pitch = Random.Range(0.9f, 1.1f);
            source.Play();
        }
    }

    void Knockback(Transform source)
    {
        Vector2 direction = source.position - transform.position;
        Vector2 norm_direction = direction.normalized;
        GetComponent<Rigidbody2D>().AddForce(-norm_direction * knockback_force);
    }

    void Die()
    {
        foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
        {
            collider.enabled = false;
        }

        CancelInvoke();

        if (dead_sprite != null)
        {
            sprite_renderer.sprite = dead_sprite;
        }

        sprite_renderer.sortingLayerName = "Corpses";

        Invoke("Fade", start_fade_time);

        Knockback(player.transform);
        camera_shake.Shake(0.1f);

        if (GetComponent<ParticleSystem>() != null)
        {
            GetComponent<ParticleSystem>().Play();
        }
    }

    void Fade()
    {
        fading = true;
    }

    public void SetRoomPosition(Vector3 max_pos, Vector3 min_pos)
    {
        this.max_pos = max_pos - Vector3.one;
        this.min_pos = min_pos + Vector3.one;
    }

    bool IsOutOfBounds()
    {
        if (transform.position.x > max_pos.x + 10 ||
            transform.position.x < min_pos.x - 10 ||
            transform.position.y > max_pos.y + 10 ||
            transform.position.y < min_pos.y - 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void EmergencyKill()
    {
        Debug.LogWarning(name + " is out bounds, killing now");
        health -= 10000;
        Die();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Running into player
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (!player.invincible && (TimeChange.current.dimension == enemy_dimension || is_boss))
            {
                player.DamagePlayer(transform);
            }
        }
        else if (collision.tag == "Wall" && GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
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

    public Vector3 GetRandomPositionInRoom()
    {
        return new Vector3(
            Random.Range(min_pos.x, max_pos.x),
            Random.Range(min_pos.y, max_pos.y),
            0);
    }

    public enum DamageType
    {
        Sword,
        Gun
    }
}
