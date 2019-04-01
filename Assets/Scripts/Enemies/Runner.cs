using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public float shift_attack_delay;
    public float speed;
    float max_speed;

    int animation_ind = 0;
    public List<Sprite> skull_animation;

    bool death_playing = false;
    public AudioClip death_clip;

    Vector3 random_target;

    Player player;
    Enemy enemy;

    SpriteRenderer sprite_renderer;


    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        max_speed = speed;
        speed = max_speed / 2f;

        random_target = enemy.GetRandomPositionInRoom();

        animation_ind = Random.Range(0, skull_animation.Count);
        StartCoroutine(SkullAnimation());
    }

    void Update()
    {
        speed = Mathf.Clamp(speed + 0.01f, 0, max_speed);

        SetSprite();

        if (!Hitstop.current.Hitstopped &&
            enemy.health > 0)
        {
            if (!player.invincible && TimeChange.current.dimension == enemy.enemy_dimension)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, random_target, speed * speed * Time.deltaTime);
            }
        }

        if (enemy.health <= 0 &&
             !death_playing)
        {
            death_playing = true;
            GetComponent<AudioSource>().clip = death_clip;
            GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().Play();
        }
    }

    void SetSprite()
    {
        if (TimeChange.current.dimension == enemy.enemy_dimension && 
            enemy.health > 0)
        {
            Vector3 check = transform.position - player.transform.position;
            if (check.x > 0)
            {
                sprite_renderer.flipX = true;
            }
            else
            {
                sprite_renderer.flipX = false;
            }
        }

        if (enemy.flashing)
        {
            sprite_renderer.sprite = enemy.flash_sprite;
        }
        else
        {
            sprite_renderer.sprite = skull_animation[animation_ind];
        }
    }

    IEnumerator SkullAnimation()
    {
        while (enemy.health > 0)
        {
            animation_ind++;
            if (animation_ind == skull_animation.Count)
            {
                animation_ind = 0;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
