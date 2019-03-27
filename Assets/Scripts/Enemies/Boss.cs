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
    float last_shot = 0f;
    float shot_cooldown = 0.4f;

    // Intensity
    int orig_health;
    bool critical = true;

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

        ChooseAttack();
    }

    void ChooseAttack()
    {
        current_attack = Attacks.Dive;
        speed = 0;
        if (transform.position.x >= 0)
        {
            dive_choice = Random.Range(0, dive_positions.Count / 2);
        }
        else
        {
            dive_choice = Random.Range(dive_positions.Count / 2, dive_positions.Count);
        }

        // Assess if health is below half
        if (enemy.health < (orig_health / 2f))
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
    }

    void ChooseAttackLogic()
    {
        switch (current_attack)
        {
            case Attacks.None:
                ChooseAttack();
                break;
            case Attacks.Dive:
                Dive();
                break;
        }
    }

    void Dive()
    {
        speed += 0.008f;

        transform.position = Vector3.MoveTowards(transform.position, dive_positions[dive_choice], speed * speed * speed * Time.deltaTime);
        if (critical) Shoot();

        if (Vector3.Distance(transform.position, dive_positions[dive_choice]) < 0.1f)
        {
            speed = orig_speed;
            current_attack = Attacks.None;
        }
    }

    void Shoot()
    {
        // Shooting
        if (Time.timeSinceLevelLoad > last_shot + shot_cooldown &&
            enemy.CanSeePlayer())
        {
            last_shot = Time.timeSinceLevelLoad;

            GameObject bullet = PlayerBulletPool.current.GetPooledBullet();
            bullet.SetActive(true);
            bullet.transform.position = transform.position;

            bullet.GetComponent<Bullet>().side = BulletSide.Enemy;
            bullet.GetComponent<Bullet>().SetSpriteAndSpeed();

            Vector3 direction = FindObjectOfType<Player>().transform.position - transform.position;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * bullet.GetComponent<Bullet>().speed;
        }
    }

    void SetSprite()
    {
        if (enemy.flashing)
        {
            sprite_renderer.sprite = enemy.flash_sprite;
        }
        else
        {
            sprite_renderer.sprite = enemy.normal_sprite;
        }
    }

    public enum Attacks
    {
        None,
        Dive
    }
}
