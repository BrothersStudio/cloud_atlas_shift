using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float speed;

    float last_shot = 0;
    float shot_cooldown = 1f;
    float shift_attack_delay = 0.5f;

    public AudioClip enemy_shot;

    Enemy enemy;
    Player player;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        // Shooting
        if (TimeChange.current.dimension == enemy.enemy_dimension && 
            (Time.timeSinceLevelLoad > last_shot + shot_cooldown) && 
            enemy.health > 0 &&
            Time.timeSinceLevelLoad > shift_attack_delay + TimeChange.current.last_change_time)
        {
            last_shot = Time.timeSinceLevelLoad;

            GameObject bullet = PlayerBulletPool.current.GetPooledBullet();
            bullet.SetActive(true);
            bullet.transform.position = transform.position;

            bullet.GetComponent<Bullet>().side = BulletSide.Enemy;
            bullet.GetComponent<Bullet>().SetSpriteAndSpeed();

            Vector3 direction = FindObjectOfType<Player>().transform.position - transform.position;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * bullet.GetComponent<Bullet>().speed;

            GetComponent<AudioSource>().clip = enemy_shot;
            GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().Play();
        }

        // Moving
        if (TimeChange.current.dimension == enemy.enemy_dimension &&
                enemy.health > 0 &&
                Time.timeSinceLevelLoad > shift_attack_delay + TimeChange.current.last_change_time)
        {
            Vector3 direction = player.transform.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * speed * Time.deltaTime;
        }
        else if (TimeChange.current.dimension != enemy.enemy_dimension)
        {
            Invoke("StopSoon", 0.5f);
        }
    }

    void StopSoon()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}

