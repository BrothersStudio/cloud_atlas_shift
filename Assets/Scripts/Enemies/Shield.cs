using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shift_attack_delay;

    Enemy enemy;
    Player player;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        // Moving
        if (TimeChange.current.dimension == enemy.enemy_dimension &&
                enemy.health > 0 &&
                Time.timeSinceLevelLoad > shift_attack_delay + TimeChange.current.last_change_time)
        {
            //Vector3 direction = player.transform.position - transform.position;
            transform.up = -(player.transform.position - transform.position);
            //transform.Rotate(new Vector3(0, 0, 90));
        }

        if (enemy.health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
