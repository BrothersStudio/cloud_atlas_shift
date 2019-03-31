using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Attacks
    Attacks current_attack;

    // Dive attack
    float orig_speed;
    float speed = 4;
    int dive_choice = 0;
    public List<Vector3> dive_positions;

    // Shooting
    float shot_position_reached_time = 0f;
    float phase_time = 8f;

    float last_quick_shot = 0f;
    float last_critical_shot = 0f;
    float quick_shot_cooldown = 0.4f;
    float critical_shot_cooldown = 1;

    int shoot_choice = 0;
    public List<Vector3> shoot_positions;

    // Intensity
    int orig_health;
    bool critical = false;

    // Sprites
    int walk_cycle = 0;
    public List<Sprite> sprites;

    int num_clouds = 3;
    public GameObject cloud_trail;
    List<GameObject> cloud_trail_pool = new List<GameObject>();

    bool playing_animation = false;
    bool moving_to_die = false;
    public Vector3 death_position;
    public List<Sprite> death_animation_sprites;
    bool done_death_animation = false;
    bool ending = false;

    Enemy enemy;
    Player player;
    SpriteRenderer sprite_renderer;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        orig_speed = speed;
        orig_health = enemy.health;

        // Populate cloud trail pool
        for (int i = 0; i < num_clouds; i++)
        {
            GameObject obj = Instantiate(cloud_trail) as GameObject;
            obj.SetActive(false);
            cloud_trail_pool.Add(obj);
        }

        ChooseAttack();
        StartCoroutine(WalkCycle());
    }

    void ChooseAttack()
    {
        speed = 0;
        if (Random.Range(0f, 1f) < 0.5)
        {
            current_attack = Attacks.Dive;

            // Choose location further away from yourself
            if (transform.position.x >= 0)
            {
                dive_choice = Random.Range(0, dive_positions.Count / 2);
            }
            else
            {
                dive_choice = Random.Range(dive_positions.Count / 2, dive_positions.Count);
            }
        }
        else
        {
            current_attack = Attacks.Shoot;
            shoot_choice = Random.Range(0, 4);
        }

        // Assess if health is below half
        if (enemy.health < (orig_health / 2f) && !critical)
        {
            critical = true;
        }
    }

    void Update()
    {
        if (Hitstop.current.Hitstopped)
        {
            return;
        }

        if (enemy.health > 0)
        {
            ChooseAttackLogic();

            SetSprite();
        }
        else if (!moving_to_die)
        {
            StopAllCoroutines();

            speed = 1;
            moving_to_die = true;

            FindObjectOfType<LevelController>().GameOver();
            FindObjectOfType<MusicController>().SetVictoryMusic();

            GetComponent<BoxCollider2D>().enabled = false;
            player.GetComponent<EdgeCollider2D>().enabled = false;
        }
        else if (!done_death_animation)
        {
            speed += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, death_position, speed * speed * speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, death_position) < 0.1f && !playing_animation)
            {
                playing_animation = true;
                StartCoroutine(PlayDeathAnimation());
            }
            else if (!playing_animation)
            {
                sprite_renderer.sprite = sprites[walk_cycle];
            }
        }
        else if (!ending)
        {
            ending = true;
            FindObjectOfType<FadeToBlack>().FadeOut();
        }
    }

    IEnumerator PlayDeathAnimation()
    {
        for (int i = 0; i < death_animation_sprites.Count; i++)
        {
            sprite_renderer.sprite = death_animation_sprites[i];
            yield return new WaitForSeconds(0.15f);
        }
        done_death_animation = true;
    }

    void ChooseAttackLogic()
    {
        switch (current_attack)
        {
            case Attacks.None:
                ChooseAttack();
                break;
            case Attacks.Dive:
                DiveAttack();
                break;
            case Attacks.Shoot:
                ShootAttack();
                break;
        }
    }

    void DiveAttack()
    {
        speed += Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, dive_positions[dive_choice], speed * speed * speed * Time.deltaTime);
        if (critical &&
            Time.timeSinceLevelLoad > last_quick_shot + quick_shot_cooldown &&
            enemy.CanSeePlayer())
        {
            last_quick_shot = Time.timeSinceLevelLoad;
            Shoot(player.transform.position);
        }

        if (Vector3.Distance(transform.position, dive_positions[dive_choice]) < 0.1f)
        {
            current_attack = Attacks.None;
        }
    }

    void ShootAttack()
    {
        if (Vector3.Distance(transform.position, shoot_positions[shoot_choice]) > 0.1f)
        {
            speed += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, shoot_positions[shoot_choice], speed * speed * speed * Time.deltaTime);

            shot_position_reached_time = Time.timeSinceLevelLoad;
        }
        else if (Time.timeSinceLevelLoad < shot_position_reached_time + phase_time)
        {
            if (critical) critical_shot_cooldown = 0.5f;

            if (Time.timeSinceLevelLoad > last_critical_shot + critical_shot_cooldown)
            {
                last_critical_shot = Time.timeSinceLevelLoad;
                for (float i = -Mathf.PI / 4f; i < 3 * Mathf.PI / 4f; i += 0.05f)
                {
                    switch (shoot_choice)
                    {
                        case 0:
                            Shoot(transform.TransformPoint(new Vector3(i, -Mathf.Cos(i), 0)));
                            break;
                        case 1:
                            Shoot(transform.TransformPoint(new Vector3(-i, -Mathf.Cos(i), 0)));
                            break;
                        case 2:
                            Shoot(transform.TransformPoint(new Vector3(-i, Mathf.Cos(i), 0)));
                            break;
                        case 3:
                            Shoot(transform.TransformPoint(new Vector3(i, Mathf.Cos(i), 0)));
                            break;
                    }
                }
            }
        }
        else
        {
            current_attack = Attacks.None;
        }
    }

    void Shoot(Vector3 location)
    {
        GameObject bullet = PlayerBulletPool.current.GetPooledBullet();
        bullet.SetActive(true);
        bullet.transform.position = transform.position;

        bullet.GetComponent<Bullet>().side = BulletSide.Enemy;
        bullet.GetComponent<Bullet>().SetSpriteAndSpeed();

        Vector3 direction = location - transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * bullet.GetComponent<Bullet>().speed;
    }

    void SetSprite()
    {
        if (enemy.flashing)
        {
            sprite_renderer.sprite = enemy.flash_sprite;
        }
        else
        {
            sprite_renderer.sprite = sprites[walk_cycle];
        }
    }

    IEnumerator WalkCycle()
    {
        while (enemy.health > 0)
        {
            walk_cycle = 0;
            SpawnCloud();
            yield return new WaitForSeconds(0.25f);
            SpawnCloud();
            yield return new WaitForSeconds(0.25f);
            walk_cycle = 1;
            SpawnCloud();
            yield return new WaitForSeconds(0.25f);
            SpawnCloud();
            yield return new WaitForSeconds(0.25f);
            walk_cycle = 2;
            SpawnCloud();
            yield return new WaitForSeconds(0.25f);
            SpawnCloud();
            yield return new WaitForSeconds(0.25f);
        }
    }

    void SpawnCloud()
    {
        GameObject cloud = GetPooledCloud();
        cloud.transform.position = transform.position;
        cloud.SetActive(true);
    }

    GameObject GetPooledCloud()
    {
        for (int i = 0; i < cloud_trail_pool.Count; i++)
        {
            if (!cloud_trail_pool[i].activeInHierarchy)
            {
                return cloud_trail_pool[i];
            }
        }

        GameObject obj = Instantiate(cloud_trail) as GameObject;
        cloud_trail_pool.Add(obj);
        return obj;
    }

    public enum Attacks
    {
        None,
        Dive,
        Shoot
    }
}
