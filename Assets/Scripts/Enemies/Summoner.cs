using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    public float shift_attack_delay;
    public float speed;

    float last_movement_change = 0;
    float change_cooldown = 1f;

    float last_summon = -4f;
    float summon_cooldown = 4f;
    public GameObject enemy_to_summon;
    public int num_to_summon;

    Player player;
    Enemy enemy;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (enemy.health > 0 && Time.timeSinceLevelLoad > change_cooldown + last_movement_change)
        {
            last_movement_change = Time.timeSinceLevelLoad;
            GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(0, 10), Random.Range(0, 10)).normalized * speed * Time.deltaTime;
        }

        if (TimeChange.current.dimension == enemy.enemy_dimension &&
            enemy.health > 0 &&
            Time.timeSinceLevelLoad > shift_attack_delay + TimeChange.current.last_change_time &&
            Time.timeSinceLevelLoad > summon_cooldown + last_summon)
        {
            last_summon = Time.timeSinceLevelLoad;

            Vector3 summoner_pos = transform.position;
            for (int i = 0; i < num_to_summon; i++)
            {
                Vector3 summoned_pos = summoner_pos;
                summoned_pos.x += Random.Range(-1, 1);
                summoned_pos.y += Random.Range(-1, 1);

                Instantiate(enemy_to_summon, summoned_pos, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            // Turn around if we hit a wall
            last_movement_change = Time.timeSinceLevelLoad;
            Vector3 direction = transform.position - other.transform.position;
            GetComponent<Rigidbody2D>().velocity = direction.normalized * speed * Time.deltaTime;
        }
    }
}
