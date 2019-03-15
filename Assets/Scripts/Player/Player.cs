﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Combat
    Dimension dimension = Dimension.Blue;
    float last_shift = 0f;
    float shift_cooldown = 1f;

    float last_shot = 0f;
    float shot_cooldown = 0.2f;

    Sword sword;
    float last_swing = 0f;
    float swing_cooldown = 0.25f;

    // Movement
    float acceleration = 0.0025f;
    
    float x_speed = 0;
    float y_speed = 0;

    float max_speed = 0.1f;
    float friction = 0.06f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        sword = transform.Find("Sword").GetComponent<Sword>();
    }

    void Update()
    {
        // Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Time.timeSinceLevelLoad > last_shift + shift_cooldown)
            {
                if (dimension == Dimension.Blue)
                {
                    dimension = Dimension.Orange;
                }
                else
                {
                    dimension = Dimension.Blue;
                }
                last_shift = Time.timeSinceLevelLoad;
            }
        }

        // Attack
        if (Input.GetMouseButton(0))
        {
            // Gun
            if (dimension == Dimension.Blue && (Time.timeSinceLevelLoad > last_shot + shot_cooldown))
            {
                last_shot = Time.timeSinceLevelLoad;

                GameObject bullet = PlayerBulletPool.current.GetPooledBullet();
                bullet.SetActive(true);
                bullet.transform.position = transform.position;

                Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * bullet.GetComponent<Bullet>().speed;

                GetComponent<PlayerSprite>().ForceSpriteForTime(0.2f);
            }
            // Sword
            else if (dimension == Dimension.Orange && (Time.timeSinceLevelLoad > last_swing + swing_cooldown))
            {
                last_swing = Time.timeSinceLevelLoad;
                sword.Swing();

                GetComponent<PlayerSprite>().ForceSpriteForTime(0.2f);
            }
        }

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

        transform.position = new Vector3(transform.position.x + x_speed, transform.position.y + y_speed, transform.position.z);
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
}

public enum Dimension
{
    Blue,
    Orange
}

public enum FacingDirection
{
    Up,
    Down,
    Left,
    Right
}