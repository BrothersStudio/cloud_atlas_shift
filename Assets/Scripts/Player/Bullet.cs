using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool homing;

    bool hit_wall = false;

    int damage = 15;
    public float player_speed;
    public float enemy_speed;

    [HideInInspector]
    public float speed;
    [HideInInspector]
    public BulletSide side;
    [HideInInspector]
    public Dimension bullet_dimension;

    public Sprite player_sprite;
    List<Sprite> current_enemy_sprites;
    public List<Sprite> enemy_blue_sprites;
    public List<Sprite> enemy_orange_sprites;

    float enable_time = 0;
    float wall_encounter_time = 0.05f;
    float self_destruct_time = 20;

    Player player;
    Rigidbody2D rigid;
    SpriteRenderer sprite_renderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();

        TimeChange.current.bullets.Add(this);
        player = FindObjectOfType<Player>();
    }

    void OnEnable()
    {
        enable_time = Time.timeSinceLevelLoad;
        sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);

        hit_wall = false;
    }

    public void SetSpriteAndSpeed()
    {
        if (side == BulletSide.Player)
        {
            speed = player_speed;
            sprite_renderer.sprite = player_sprite;
        }
        else
        {
            speed = enemy_speed;
            bullet_dimension = TimeChange.current.dimension;
            if (TimeChange.current.dimension == Dimension.Blue)
            {
                current_enemy_sprites = enemy_blue_sprites;
            }
            else
            {
                current_enemy_sprites = enemy_orange_sprites;
            }
            StartCoroutine(CycleSprite());
        }
    }

    IEnumerator CycleSprite()
    {
        while (true)
        {
            sprite_renderer.sprite = current_enemy_sprites[0];
            yield return new WaitForSeconds(0.2f);
            sprite_renderer.sprite = current_enemy_sprites[1];
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SwitchDimensions()
    {
        if (TimeChange.current.dimension != bullet_dimension)
        {
            sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 0.3f);
        }
        else
        {
            sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);
        }
    }

    void Update()
    {
        if (Hitstop.current.Hitstopped)
        {
            rigid.simulated = false;
        }
        else if (rigid.simulated == false)
        {
            rigid.simulated = true;

        }

        if (rigid.simulated && homing)
        {
            if (TimeChange.current.dimension == bullet_dimension)
            {
                Vector3 direction = player.transform.position - transform.position;
                GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * speed / 2f;
            }
        }

        if (Time.timeSinceLevelLoad > enable_time + self_destruct_time)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && side == BulletSide.Enemy)
        {
            Player player = collision.GetComponent<Player>();
            if (TimeChange.current.dimension == bullet_dimension && !player.invincible)
            {
                Disappear();
                player.DamagePlayer(transform);
            }
        }
        else if (collision.tag == "Enemy" && side == BulletSide.Player)
        {
            if (collision.GetComponent<Enemy>().enemy_dimension == Dimension.Blue || collision.GetComponent<Enemy>().is_boss)
            {
                Disappear();
                collision.GetComponent<Enemy>().GunHit(damage);
            }
        }
        else if (collision.tag == "Shield" && side == BulletSide.Player)
        {
            collision.GetComponent<Shield>().HitShield();
            HitWall();
        }
        else if (collision.tag == "Wall" &&
            Time.timeSinceLevelLoad > wall_encounter_time + enable_time &&
            !hit_wall)
        {
            HitWall();
        }
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }

    void HitWall()
    {
        hit_wall = true;

        GameObject wall_hit = PlayerBulletPool.current.GetPooledWallHit();
        wall_hit.transform.position = transform.position;
        RotateWallHit(wall_hit);
        wall_hit.SetActive(true);

        if (TimeChange.current.dimension != bullet_dimension)
        {
            wall_hit.GetComponent<SpriteRenderer>().color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 0.3f);
        }
        else
        {
            wall_hit.GetComponent<SpriteRenderer>().color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);
        }

        gameObject.SetActive(false);
    }

    void RotateWallHit(GameObject wall_hit)
    {
        wall_hit.transform.rotation = Quaternion.identity;

        float angle = 0;
        Vector2 vec = GetComponent<Rigidbody2D>().velocity;
        if (vec.x < 0)
        {
            angle = 360 - (Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            angle = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;
        }

        wall_hit.transform.Rotate(new Vector3(0, 0, -angle));

        if (angle > 45 && angle < 305)
        {
            wall_hit.transform.Translate(new Vector3(0, 0.8f, 0));
        }
    }
}

public enum BulletSide
{
    Player,
    Enemy
}
