using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    bool firing = false;
    float charge_time = 0f;
    float last_shot;

    public AudioClip charge_up;
    public AudioClip fire_shot;
    AudioSource source;

    bool blinking = false;
    Transform child;
    SpriteRenderer laser_renderer;
    public Color charging_color;
    public Color firing_color;

    int sprite_ind = 0;
    public List<Sprite> normal_sprites;
    public List<Sprite> charging_sprites;

    Player player;
    Enemy enemy;
    SpriteRenderer sprite_renderer;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
        sprite_renderer = GetComponent<SpriteRenderer>();

        child = transform.GetChild(0);
        laser_renderer = child.GetComponent<SpriteRenderer>();

        source = GetComponent<AudioSource>();

        last_shot = -fire_shot.length;
    }

    void Update()
    {
        if (Hitstop.current.Hitstopped)
        {
            return;
        }

        if (TimeChange.current.dimension == GetComponentInParent<Enemy>().enemy_dimension &&
            enemy.health > 0 &&
            enemy.CanSeePlayer() &&
            Time.timeSinceLevelLoad > last_shot + fire_shot.length)
        {
            if (charge_time <= 0.1f && !source.isPlaying)
            {
                source.clip = charge_up;
                source.pitch = Random.Range(0.9f, 1.1f);
                source.Play();
            }

            StartCoroutine(LaserBlink());
            laser_renderer.color = charging_color;

            child.right = player.transform.position - transform.position;
            child.localPosition = (player.transform.position - transform.position + new Vector3(0, 0.6f, 0)) / 2f;
            child.localScale = new Vector3(Vector3.Distance(transform.position + new Vector3(0, 0.6f, 0), player.transform.position) * 16, 1, 1);

            charge_time += Time.deltaTime;

            if (charge_time > 3)
            {
                firing = true;

                source.clip = fire_shot;
                source.pitch = Random.Range(0.9f, 1.1f);
                source.Play();

                charge_time = 0;

                player.DamagePlayer(transform);

                last_shot = Time.timeSinceLevelLoad;
            }
        }
        else if (firing &&
            TimeChange.current.dimension == GetComponentInParent<Enemy>().enemy_dimension &&
            enemy.CanSeePlayer())
        {
            blinking = false;
            StopCoroutine(LaserBlink());

            laser_renderer.enabled = true;
            laser_renderer.color = firing_color;
            child.right = player.transform.position - transform.position;
            child.localPosition = (player.transform.position - transform.position + new Vector3(0, 0.6f, 0)) / 2f;
            child.localScale = new Vector3(Vector3.Distance(transform.position + new Vector3(0, 0.6f, 0), player.transform.position) * 16, 3, 1);
        }
        else if (firing &&
            !enemy.CanSeePlayer())
        {
            firing = false;
        }
        else 
        {
            blinking = false;
            StopCoroutine(LaserBlink());
            laser_renderer.enabled = false;

            firing = false;
            charge_time = 0;

            source.Stop();
        }

        SetSprite();
    }

    IEnumerator LaserBlink()
    {
        if (!blinking)
        {
            blinking = true;
            for (int i = 0; i < 2; i++)
            {
                laser_renderer.enabled = !laser_renderer.enabled;
                yield return new WaitForSeconds(0.5f);
            }

            for (int i = 0; i < 5; i++)
            {
                laser_renderer.enabled = !laser_renderer.enabled;
                yield return new WaitForSeconds(0.25f);
            }

            for (int i = 0; i < 10; i++)
            {
                laser_renderer.enabled = !laser_renderer.enabled;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void SetSprite()
    {
        if (enemy.flashing)
        {
            sprite_renderer.sprite = enemy.flash_sprite;
            return;
        }

        if (TimeChange.current.dimension != GetComponentInParent<Enemy>().enemy_dimension)
        {
            sprite_renderer.sprite = normal_sprites[0];
            return;
        }

        Vector3 check = transform.position - player.transform.position;
        if (check.x > 0)
        {
            sprite_renderer.flipX = true;
        }
        else
        {
            sprite_renderer.flipX = false;
        }

        if (check.y > 0)
        {
            sprite_ind = 0;
        }
        else
        {
            sprite_ind = 1;
        }

        if (charge_time > 0)
        {
            sprite_renderer.sprite = normal_sprites[sprite_ind];
        }
        else
        {
            sprite_renderer.sprite = charging_sprites[sprite_ind];
        }
    }
}
