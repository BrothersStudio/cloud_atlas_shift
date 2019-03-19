using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shift_attack_delay;

    public AudioClip hit_shield;

    Enemy enemy;
    Player player;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        // Facing direction
        if (TimeChange.current.dimension == enemy.enemy_dimension &&
                enemy.health > 0 &&
                Time.timeSinceLevelLoad > shift_attack_delay + TimeChange.current.last_change_time)
        {
            transform.up = -(player.transform.position - transform.position);
        }

        if (enemy.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void HitShield()
    {
        GetComponent<AudioSource>().clip = hit_shield;
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
    }
}
