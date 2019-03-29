using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float current_speed = 4f;
    int num_beams = 1;

    public bool is_vertical;

    Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        if (is_vertical)
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }

        while (true)
        {
            bool done_making_lasers = false;
            Collider2D[] check = Physics2D.OverlapCircleAll(transform.TransformPoint(new Vector2(-1 - num_beams, 0)), 0.1f);
            foreach (Collider2D collider in check)
            {
                if (collider.tag == "Wall")
                {
                    done_making_lasers = true;
                }
            }

            if (!done_making_lasers)
            {
                GameObject new_beam = Instantiate(transform.GetChild(0).gameObject, transform.TransformPoint(new Vector2(-1 - num_beams, 0)), Quaternion.identity);
                new_beam.transform.SetParent(transform);
                if (is_vertical)
                {
                    new_beam.transform.Rotate(new Vector3(0, 0, 90));
                }

                num_beams++;
            }
            else
            {
                break;
            }

            // For emergencies
            if (num_beams == 20)
            {
                Debug.LogError("That's a really long laser, I think something broke");
                break;
            }
        }
    }

    void Update()
    {
        if (Hitstop.current.Hitstopped || enemy.health <= 0)
        {
            return;
        }

        Collider2D[] top_colliders = Physics2D.OverlapCircleAll(transform.TransformPoint(new Vector2(0, 0.2f)), 0.1f);
        foreach (Collider2D collider in top_colliders)
        {
            if (collider.tag == "Wall")
            {
                current_speed = -current_speed;
                break;
            }
        }

        Collider2D[] bot_colliders = Physics2D.OverlapCircleAll(transform.TransformPoint(new Vector2(0, -0.2f)), 0.1f);
        foreach (Collider2D collider in bot_colliders)
        {
            if (collider.tag == "Wall")
            {
                current_speed = -current_speed;
                break;
            }
        }

        Vector2 new_pos = transform.position;
        if (is_vertical)
        {
            new_pos.x -= current_speed * Time.deltaTime;
        }
        else
        {
            new_pos.y -= current_speed * Time.deltaTime;
        }
        transform.position = new_pos;
    }
}
