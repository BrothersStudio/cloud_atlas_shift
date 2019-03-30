using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public float shift_attack_delay;
    public float speed;
    float max_speed;

    bool animating = true;
    int animation_ind = 0;
    public List<Sprite> skull_animation;

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
        speed = 0;

        random_target = enemy.GetRandomPositionInRoom();

        animation_ind = Random.Range(0, skull_animation.Count);
        StartCoroutine(SkullAnimation());
    }

    void Update()
    {
        speed = Mathf.Clamp(speed + 0.01f, 0, max_speed);

        SetSprite();

        if (!player.invincible && TimeChange.current.dimension == enemy.enemy_dimension)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, random_target, speed * speed * Time.deltaTime);
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
            animating = true;
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
