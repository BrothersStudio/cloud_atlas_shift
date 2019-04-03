using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFloat : MonoBehaviour
{
    Vector3 orig_position;
    Vector3 float_position;

    float move_speed = 0.5f;
    float move_dist = 0.05f;

    bool start_float = false;

    void Start()
    {
        orig_position = transform.position;
        float_position = orig_position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0).normalized * move_dist;

        Invoke("StartFloat", Random.Range(0, 1f));
    }

    void StartFloat()
    {
        start_float = true;
    }

    void Update()
    {
        if (start_float)
        {
            transform.position = Vector3.Lerp(transform.position, float_position, Time.deltaTime * move_speed);
            if (Vector3.Distance(transform.position, float_position) < 0.01f)
            {
                if (float_position == orig_position)
                {
                    float_position = orig_position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0).normalized * move_dist;
                }
                else
                {
                    float_position = orig_position;
                }
            }
        }
    }
}
