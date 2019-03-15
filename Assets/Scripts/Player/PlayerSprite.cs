using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    float stick_time = 0;

    int walk_sprite_ind = 0;
    float walk_sprite_time = 0;
    float walk_sprite_flip_time = 0.5f;

    public Sprite[] walking_up;
    public Sprite[] walking_down;
    public Sprite[] walking_left;
    //public Sprite[] walking_right;  probably not needed

    private SpriteRenderer sprite_renderer;

    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Facing direction
        // We do the swinging check weird like this so we can stop the feet if we have to
        if (Input.GetKey(KeyCode.W))
        {
            if (IsTimeUp())
            {
                Up();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (IsTimeUp())
            {
                Left();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (IsTimeUp())
            {
                Right();
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (IsTimeUp())
            {
                Down();
            }
        }
        else
        {
            walk_sprite_time = 0;
        }

        // Walk cycle
        walk_sprite_time += Time.deltaTime;
        if (walk_sprite_time >= walk_sprite_flip_time)
        {
            walk_sprite_time = 0;
            if (walk_sprite_ind == 0)
            {
                walk_sprite_ind = 1;
            }
            else if (walk_sprite_ind == 1)
            {
                walk_sprite_ind = 0;
            }
        }
    }

    void Up()
    {
        sprite_renderer.sprite = walking_up[walk_sprite_ind];
    }

    void Down()
    {
        sprite_renderer.sprite = walking_down[walk_sprite_ind];
    }

    void Left()
    {
        sprite_renderer.flipX = false;
        sprite_renderer.sprite = walking_left[walk_sprite_ind];
    }

    void Right()
    {
        sprite_renderer.flipX = true;
        sprite_renderer.sprite = walking_left[walk_sprite_ind];
    }

    public void ForceSpriteForTime(float time)
    {
        stick_time = Time.timeSinceLevelLoad + time;

        switch (GetComponent<Player>().GetFacingDirection())
        {
            case FacingDirection.Up:
                Up();
                break;
            case FacingDirection.Down:
                Down();
                break;
            case FacingDirection.Left:
                Left();
                break;
            case FacingDirection.Right:
                Right();
                break;
        }
    }

    bool IsTimeUp()
    {
        if (Time.timeSinceLevelLoad > stick_time)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
