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
    bool critical = false;

    // Sprites
    int walk_cycle = 0;
    public List<Sprite> sprites;

    int num_clouds = 5;
    public GameObject cloud_trail;
    List<GameObject> cloud_trail_pool = new List<GameObject>();

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
            sprite_renderer.sprite = sprites[walk_cycle];
        }
    }

    IEnumerator WalkCycle()
    {
        while (true)
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
        Dive
    }
}
