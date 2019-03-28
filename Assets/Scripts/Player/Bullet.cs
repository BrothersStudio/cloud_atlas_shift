using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool homing;

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
                current_enemy_sprites = enemy_blue_sprites;
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
            foreach (Collider2D collider in GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }
        }
        else
        {
            sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1);
            foreach (Collider2D collider in GetComponents<Collider2D>())
            {
                collider.enabled = true;
            }
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
        else if (collision.tag == "Wall")
        {
            Invoke("HitWall", 0.03f);
        }
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }

    void HitWall()
    {
        GameObject wall_hit = PlayerBulletPool.current.GetPooledWallHit();
        wall_hit.transform.position = transform.position;
        wall_hit.SetActive(true);

        gameObject.SetActive(false);
    }
}

public enum BulletSide
{
    Player,
    Enemy
}
