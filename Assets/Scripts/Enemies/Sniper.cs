using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    float charge_time = 0f;
    float last_shot;

    public AudioClip charge_up;
    public AudioClip fire_shot;
    AudioSource source;

    Transform child;
    SpriteRenderer laser_renderer;

    Player player;
    Enemy enemy;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();

        child = transform.GetChild(0);
        laser_renderer = child.GetComponent<SpriteRenderer>();

        source = GetComponent<AudioSource>();

        last_shot = -fire_shot.length;
    }

    void Update()
    {
        if (TimeChange.current.dimension == GetComponentInParent<Enemy>().enemy_dimension &&
            enemy.health > 0 &&
            enemy.CanSeePlayer() &&
            Time.timeSinceLevelLoad > last_shot + fire_shot.length)
        {
            if (charge_time <= 0.1f && !source.isPlaying)
            {
                source.clip = charge_up;
                source.pitch = Random.Range(0.9f, 1.1f);
                source.Play();
            }

            laser_renderer.enabled = true;

            child.right = player.transform.position - transform.position;
            child.localPosition = (player.transform.position - transform.position) / 2f;
            child.localScale = new Vector3(Vector3.Distance(transform.position, player.transform.position) * 16, 1, 1);

            charge_time += Time.deltaTime;

            if (charge_time > 3)
            {
                source.clip = fire_shot;
                source.pitch = Random.Range(0.9f, 1.1f);
                source.Play();

                charge_time = 0;

                player.DamagePlayer(transform);

                last_shot = Time.timeSinceLevelLoad;
            }
        }
        else
        {
            laser_renderer.enabled = false;

            charge_time = 0;

            source.Stop();
        }
    }
}
