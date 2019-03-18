using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public float shift_attack_delay;
    public float speed;

    Player player;
    Enemy enemy;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (TimeChange.current.dimension == enemy.enemy_dimension && 
            enemy.health > 0 &&
            Time.timeSinceLevelLoad > shift_attack_delay + TimeChange.current.last_change_time)
        {
            Vector3 direction = player.transform.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * speed * Time.deltaTime;
        }
        else if (TimeChange.current.dimension != enemy.enemy_dimension)
        {
            Invoke("StopSoon", 0.5f);
        }
    }

    void StopSoon()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
