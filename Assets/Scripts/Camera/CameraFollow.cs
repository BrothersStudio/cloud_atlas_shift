using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float follow_speed = 0.1f;
    public Transform player;

    float trauma = 0;

    float max_angle = 1f;
    float max_offset = 0.5f;

    Vector3 default_position;
    Quaternion default_rotation;

    void Start()
    {
        default_rotation = transform.rotation;
    }

    public void Shake(float amount)
    {
        trauma += amount;
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
    }
}
