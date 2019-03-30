using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CameraFollow main_camera;

    // Combat
    public int health = 3;
    public GameObject heart_bar;
    public bool dying = false;

    [HideInInspector]
    public bool invincible = false;
    float invincible_stop_time = 0f;
    float invincible_time = 2f;

    public bool knockback_state = false;
    float knockback_force = 1000f;
    float knockback_time = 0.05f;

    float last_shift = -0.5f;
    float shift_cooldown = 0.25f;

    float last_shot = -0.2f;
    float shot_cooldown = 0.2f;

    Sword sword;
    float last_swing = -0.50f;
    float swing_cooldown = 0.50f;

    // Movement
    float acceleration = 500f;
    
    float x_speed = 0;
    float y_speed = 0;

    float max_speed = 1200f;
    float friction = 0.06f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        main_camera = Camera.main.GetComponent<CameraFollow>();
        sword = transform.Find("Sword").GetComponent<Sword>();
    }

    void Update()
    {
        // Make sure we aren't frozen
        if (Hitstop.current.Hitstopped)
        {
            GetComponent<Rigidbody2D>().simulated = false;
            return;
        }
        else
        {
            GetComponent<Rigidbody2D>().simulated = true;
        }

        // Dying
        if (dying)
        {
            return;
        }

        // Recovering from damage
        if (invincible)
        {
            if (Time.timeSinceLevelLoad > invincible_stop_time)
            {
                invincible = false;
            }
        }

        // Shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
        {
            if (Time.timeSinceLevelLoad > last_shift + shift_cooldown)
            {
                TimeChange.current.Switch();
                last_shift = Time.timeSinceLevelLoad;
            }
        }

        // Attack
        if (Input.GetMouseButton(0))
        {
            // Gun
            if (TimeChange.current.dimension == Dimension.Blue && (Time.timeSinceLevelLoad > last_shot + shot_cooldown))
            {
                last_shot = Time.timeSinceLevelLoad;

                GameObject bullet = PlayerBulletPool.current.GetPooledBullet();
                bullet.SetActive(true);
                bullet.transform.position = transform.position;

                bullet.GetComponent<Bullet>().side = BulletSide.Player;
                bullet.GetComponent<Bullet>().SetSpriteAndSpeed();

                Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * bullet.GetComponent<Bullet>().speed;

                GetComponent<PlayerSprite>().ForceSpriteForTime(0.2f);
                GetComponent<PlayerAudio>().Shoot();
            }
            // Sword
            else if (TimeChange.current.dimension == Dimension.Orange && (Time.timeSinceLevelLoad > last_swing + swing_cooldown))
            {
                last_swing = Time.timeSinceLevelLoad;
                sword.Swing();

                GetComponent<PlayerSprite>().ForceSpriteForTime(0.2f);
                GetComponent<PlayerAudio>().Slash();
            }
        }

        if (!knockback_state && Input.anyKey)
        {
            // Movement
            if (Input.GetKey(KeyCode.W))
            {
                y_speed = Mathf.Clamp(y_speed + acceleration, -max_speed, max_speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                x_speed = Mathf.Clamp(x_speed - acceleration, -max_speed, max_speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                x_speed = Mathf.Clamp(x_speed + acceleration, -max_speed, max_speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                y_speed = Mathf.Clamp(y_speed - acceleration, -max_speed, max_speed);
            }

            x_speed *= (1 - friction);
            y_speed *= (1 - friction);

            //transform.position = new Vector3(transform.position.x + x_speed * Time.deltaTime, transform.position.y + y_speed * Time.deltaTime, transform.position.z);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(x_speed * Time.deltaTime, y_speed * Time.deltaTime));
        }
        else if (!knockback_state)
        {
            x_speed *= (1 - friction);
            y_speed *= (1 - friction);
        }
    }

    public void ResetSpeed()
    {
        x_speed = 0;
        y_speed = 0;
    }

    public FacingDirection GetFacingDirection()
    {
        Vector3 dir_vec = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (dir_vec.y > 0)
        {
            if (dir_vec.y > Mathf.Abs(dir_vec.x))
            {
                return FacingDirection.Up;
            }
            else
            {
                if (dir_vec.x > 0)
                {
                    return FacingDirection.Right;
                }
                else
                {
                    return FacingDirection.Left;
                }
            }
        }
        else
        {
            if (-dir_vec.y > Mathf.Abs(dir_vec.x))
            {
                return FacingDirection.Down;
            }
            else
            {
                if (dir_vec.x > 0)
                {
                    return FacingDirection.Right;
                }
                else
                {
                    return FacingDirection.Left;
                }
            }
        }
    }

    public void DamagePlayer(Transform source)
    {
        if (invincible) return;

        invincible = true;
        invincible_stop_time = invincible_time + Time.timeSinceLevelLoad;

        health--;
        heart_bar.GetComponentsInChildren<Heart>()[health].FlipHeartSprite();

        main_camera.Shake(0.5f);
        Knockback(source);

        if (health == 0)
        {
            dying = true;
            GetComponent<ParticleSystem>().Play();
            GetComponent<PlayerSprite>().Dead();
            GetComponent<PlayerAudio>().Dead();

            main_camera.PlayerDead();

            Invoke("Restart", 2f);
        }
        else
        {
            GetComponent<PlayerSprite>().Hit();
            GetComponent<PlayerAudio>().Hurt();
        }
    }

    void Knockback(Transform source)
    {
        Vector2 direction = source.position - transform.position;
        Vector2 norm_direction = direction.normalized;
        GetComponent<Rigidbody2D>().AddForce(-norm_direction * knockback_force);
        Invoke("EndKnockback", knockback_time);
        knockback_state = true;
    }

    void EndKnockback()
    {
        knockback_state = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void Restart()
    {
        FindObjectOfType<LevelController>().ResetLevel();
    }
}

public enum FacingDirection
{
    Up,
    Down,
    Left,
    Right
}
