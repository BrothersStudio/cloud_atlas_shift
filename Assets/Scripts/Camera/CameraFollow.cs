using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float follow_speed = 0.1f;
    public Transform player;

    float ortho_size;

    float trauma = 0;

    float max_angle = 0.2f;
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

    public void SetMaxMinCameraPosForRoom(List<Vector3> vectors, Transform room_loc)
    {
        max_allowed_position = room_loc.TransformPoint(vectors[0]);
        min_allowed_position = room_loc.TransformPoint(vectors[1]);
    }

    public void MoveToNextRoom(Vector3 new_room_pos)
    {
        new_pos = new_room_pos;
        new_pos.x -= +0.5f;
        new_pos.z = -10f;
        animating = true;
    }

    void Update()
    {
        trauma = Mathf.Clamp01(trauma - 0.01f);
        if (trauma == 0)
        {
            transform.rotation = default_rotation;
        }
    }

    void LateUpdate()
    {
        if (animating)
        {
            transform.position = Vector3.Lerp(transform.position, new_pos, 0.02f);
            if (Vector3.Distance(transform.position, new_pos) < 0.15f)
            {
                animating = false;
            }
        }
        else
        {
            // Might want to follow the mouse a little as well as potentially enemies, see juicing with math talk
            float new_x = transform.position.x * (1 - follow_speed) + player.position.x * follow_speed;
            float new_y = transform.position.y * (1 - follow_speed) + player.position.y * follow_speed;
            transform.position = new Vector3(new_x, new_y, -10);

            if (trauma > 0)
            {
                float angle = max_angle * Mathf.Pow(trauma, 2) * Random.Range(-1f, 1f);
                transform.Rotate(new Vector3(0, 0, angle));

                float offset_x = max_offset * Mathf.Pow(trauma, 2) * Random.Range(-1f, 1f);
                float offset_y = max_offset * Mathf.Pow(trauma, 2) * Random.Range(-1f, 1f);
                transform.Translate(new Vector2(offset_x, offset_y));
            }

            CheckForMinMaxPos();
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

        if (transform.position.x > min_allowed_position.x - ortho_size * 1.7777f)
        {
            tweaked_pos.x = max_allowed_position.x - ortho_size * 1.7777f;
        }
        else if (transform.position.x < max_allowed_position.x + ortho_size * 1.7777f)
        {
            tweaked_pos.x = min_allowed_position.x + ortho_size * 1.7777f;
        }

        transform.position = tweaked_pos;
    }
}
