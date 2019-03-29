using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float follow_speed = 0.1f;
    public Transform player;
    bool player_dead = false;

    float ortho_size;

    float trauma = 0;

    float max_angle = 0.01f;
    float max_offset = 0.5f;

    Vector3 default_position;
    Quaternion default_rotation;

    public Vector3 max_allowed_position;
    public Vector3 min_allowed_position;

    bool animating = false;
    Vector3 new_pos;

    void Start()
    {
        default_rotation = transform.rotation;

        ortho_size = GetComponent<Camera>().orthographicSize;
    }

    public void Shake(float amount)
    {
        trauma += amount;
    }

    public void SetMaxMinCameraPosForRoom(Vector3 max_pos, Vector3 min_pos)
    {
        max_allowed_position = max_pos;
        min_allowed_position = min_pos;
    }

    public void MoveToNextRoom(Vector3 new_room_pos)
    {
        new_pos = new_room_pos;
        new_pos.x -= +0.5f;
        new_pos.z = -10f;
        Hitstop.current.HitstopFor(1);
        animating = true;
    }

    void Update()
    {
        if (player_dead) return;

        trauma = Mathf.Clamp01(trauma - 0.01f);
        if (trauma == 0)
        {
            transform.rotation = default_rotation;
        }
    }

    void LateUpdate()
    {
        if (player_dead) return;

        if (animating)
        {
            transform.position = Vector3.Lerp(transform.position, new_pos, 0.025f);
            if (Vector3.Distance(transform.position, new_pos) < 0.15f)
            {
                animating = false;
            }
        }
        else
        {
            if (trauma > 0)
            {
                float angle = max_angle * trauma * trauma * Random.Range(-1f, 1f);
                transform.Rotate(new Vector3(0, 0, angle));

                float offset_x = max_offset * trauma * trauma * Random.Range(-1f, 1f);
                float offset_y = max_offset * trauma* trauma * Random.Range(-1f, 1f);
                transform.Translate(new Vector2(offset_x, offset_y));
            }
            else
            {
                float new_x = transform.position.x * (1 - follow_speed) + player.position.x * follow_speed;
                float new_y = transform.position.y * (1 - follow_speed) + player.position.y * follow_speed;
                transform.position = new Vector3(new_x, new_y, -10);

                CheckForMinMaxPos();
            }
        }
    }

    void CheckForMinMaxPos()
    {
        Vector3 tweaked_pos = transform.position;
        if (transform.position.y > max_allowed_position.y - ortho_size)
        {
            tweaked_pos.y = max_allowed_position.y - ortho_size;
        }
        else if (transform.position.y < min_allowed_position.y + ortho_size)
        {
            tweaked_pos.y = min_allowed_position.y + ortho_size;
        }

        if (transform.position.x > max_allowed_position.x - ortho_size * 1.7777f)
        {
            tweaked_pos.x = max_allowed_position.x - ortho_size * 1.7777f;
        }
        else if (transform.position.x < min_allowed_position.x + ortho_size * 1.7777f)
        {
            tweaked_pos.x = min_allowed_position.x + ortho_size * 1.7777f;
        }

        transform.position = tweaked_pos;
    }

    public void PlayerDead()
    {
        player_dead = true;

        StartCoroutine(SlowZoom());
    }

    IEnumerator SlowZoom()
    {
        while (true)
        {
            GetComponent<Camera>().orthographicSize -= 0.02f;
            transform.position = player.transform.position + new Vector3(0, 0, -10);

            yield return new WaitForSeconds(0.01f);
        }
    }
}
