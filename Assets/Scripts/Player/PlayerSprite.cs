using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    Player player;

    bool flashing = false;
    float flash_time = 0;
    float flash_speed = 0.2f;

    float sprite_force_time = 0;

    float walk_sprite_time = 0;
    float walk_sprite_flip_time = 0.5f;

    public GameObject dust_cloud;

    WalkDir player_direction = WalkDir.Up;

    // Past
    public Sprite past_walking_up;
    public Sprite past_walking_down;
    public Sprite past_walking_left;
    public Sprite past_walking_right;

    public Sprite past_flash_up;
    public Sprite past_flash_down;
    public Sprite past_flash_left;
    public Sprite past_flash_right;

    public Sprite past_walking_up_swordless;
    public Sprite past_walking_down_swordless;
    public Sprite past_walking_left_swordless;
    public Sprite past_walking_right_swordless;

    public Sprite past_flash_up_swordless;
    public Sprite past_flash_down_swordless;
    public Sprite past_flash_left_swordless;
    public Sprite past_flash_right_swordless;

    public Sprite past_dead;

    // Future
    public Sprite future_walking_up;
    public Sprite future_walking_down;
    public Sprite future_walking_left;
    public Sprite future_walking_right;

    public Sprite future_flash_up;
    public Sprite future_flash_down;
    public Sprite future_flash_left;
    public Sprite future_flash_right;

    public Sprite future_dead;

    private SpriteRenderer sprite_renderer;

    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();

        player = GetComponent<Player>();
    }

    public void Hit()
    {
        flash_time = 0;
        flashing = true;
    }

    public void Dead()
    {
        flash_time = 0;
        flashing = true;
    }

    void Update()
    {
        if (Hitstop.current.Hitstopped)
        {
            return;
        }
        else if (player.dying)
        {
            SelectDyingSprite();
            return;
        }

        // Walk cycle
        walk_sprite_time += Time.deltaTime;
        if (walk_sprite_time >= walk_sprite_flip_time)
        {
            walk_sprite_time = 0;

            Vector3 spawn_loc = transform.position;
            spawn_loc.y -= 0.18f;
            Instantiate(dust_cloud, spawn_loc, Quaternion.identity);
        }

        // Facing direction
        if (Input.GetKey(KeyCode.W))
        {
            if (IsForceTimeUp())
            {
                player_direction = WalkDir.Up;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (IsForceTimeUp())
            {
                player_direction = WalkDir.Left;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (IsForceTimeUp())
            {
                player_direction = WalkDir.Right;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (IsForceTimeUp())
            {
                player_direction = WalkDir.Down;
            }
        }
        else
        {
            walk_sprite_time = 0;
        }

        if (player.invincible || player.dying)
        {
            flash_time += Time.deltaTime;
            if (flash_time > flash_speed)
            {
                flashing = !flashing;
                flash_time = 0;
            }
        }
        else
        {
            flashing = false;
        }

        SetPlayerDirectionSprite();
    }

    void SetPlayerDirectionSprite()
    {
        switch (player_direction)
        {
            case WalkDir.Up:
                SetUpSprite();
                break;
            case WalkDir.Down:
                SetDownSprite();
                break;
            case WalkDir.Left:
                SetLeftSprite();
                break;
            case WalkDir.Right:
                SetRightSprite();
                break;
        }
    }

    void SelectDyingSprite()
    {
        if (TimeChange.current.dimension == Dimension.Blue)
        {
            sprite_renderer.sprite = future_flash_down;
        }
        else
        {
            sprite_renderer.sprite = past_flash_down;
        }
    }

    void SetUpSprite()
    {
        if (TimeChange.current.dimension == Dimension.Blue)
        {
            if (flashing)
            {
                sprite_renderer.sprite = future_flash_up;
            }
            else
            {
                sprite_renderer.sprite = future_walking_up;
            }
        }
        else if (TimeChange.current.dimension == Dimension.Orange)
        {
            if (flashing)
            {
                if (IsForceTimeUp())
                {
                    sprite_renderer.sprite = past_flash_up;
                }
                else
                {
                    sprite_renderer.sprite = past_flash_up_swordless;
                }
            }
            else
            {
                if (IsForceTimeUp())
                {
                    sprite_renderer.sprite = past_walking_up;
                }
                else
                {
                    sprite_renderer.sprite = past_walking_up_swordless;
                }
            }
        }
    }

    void SetDownSprite()
    {
        if (TimeChange.current.dimension == Dimension.Blue)
        {
            if (flashing)
            {
                sprite_renderer.sprite = future_flash_down;
            }
            else
            {
                sprite_renderer.sprite = future_walking_down;
            }
        }
        else if (TimeChange.current.dimension == Dimension.Orange)
        {
            if (flashing)
            {
                if (IsForceTimeUp())
                {
                    sprite_renderer.sprite = past_flash_down;
                }
                else
                {
                    sprite_renderer.sprite = past_flash_down_swordless;
                }
            }
            else
            {
                if (IsForceTimeUp())
                {
                    sprite_renderer.sprite = past_walking_down;
                }
                else
                {
                    sprite_renderer.sprite = past_walking_down_swordless;
                }
            }
        }
    }

    void SetLeftSprite()
    {
        if (TimeChange.current.dimension == Dimension.Blue)
        {
            if (flashing)
            {
                sprite_renderer.sprite = future_flash_left;
            }
            else
            {
                sprite_renderer.sprite = future_walking_left;
            }
        }
        else if (TimeChange.current.dimension == Dimension.Orange)
        {
            if (flashing)
            {
                if (IsForceTimeUp())
                {
                    sprite_renderer.sprite = past_flash_left;
                }
                else
                {
                    sprite_renderer.sprite = past_flash_left_swordless;
                }
            }
            else
            {
                if (IsForceTimeUp())
                {
                    sprite_renderer.sprite = past_walking_left;
                }
                else
                {
                    sprite_renderer.sprite = past_walking_left_swordless;
                }
            }
        }
    }

    void SetRightSprite()
    {
        if (TimeChange.current.dimension == Dimension.Blue)
        {
            if (flashing)
            {
                sprite_renderer.sprite = future_flash_right;
            }
            else
            {
                sprite_renderer.sprite = future_walking_right;
            }
        }
        else if (TimeChange.current.dimension == Dimension.Orange)
        {
            if (flashing)
            {
                if (IsForceTimeUp())
                {
                    sprite_renderer.sprite = past_flash_right;
                }
                else
                {
                    sprite_renderer.sprite = past_flash_right_swordless;
                }
            }
            else
            {
                if (IsForceTimeUp())
                {
                    sprite_renderer.sprite = past_walking_right;
                }
                else
                {
                    sprite_renderer.sprite = past_walking_right_swordless;
                }
            }
        }
    }

    public void ForceSpriteForTime(float time)
    {
        sprite_force_time = Time.timeSinceLevelLoad + time;
        switch (GetComponent<Player>().GetFacingDirection())
        {
            case FacingDirection.Up:
                player_direction = WalkDir.Up;
                break;
            case FacingDirection.Down:
                player_direction = WalkDir.Down;
                break;
            case FacingDirection.Left:
                player_direction = WalkDir.Left;
                break;
            case FacingDirection.Right:
                player_direction = WalkDir.Right;
                break;
        }
    }

    bool IsForceTimeUp()
    {
        if (Time.timeSinceLevelLoad > sprite_force_time)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public enum WalkDir
{
    Up,
    Down,
    Left,
    Right
}
