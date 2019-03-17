using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> enemy_to_place;
    public List<Vector3> location_to_place;

    List<Enemy> enemies = new List<Enemy>();
    bool room_complete = false;

    void Start()
    {
        for (int i = 0; i < enemy_to_place.Count; i++)
        {
            GameObject new_enemy = Instantiate(enemy_to_place[i], transform.TransformPoint(location_to_place[i]), Quaternion.identity);
            enemies.Add(new_enemy.GetComponent<Enemy>());
        }
    }

    void Update()
    {
        if (!room_complete)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.health > 0)
                {
                    return;
                }
            }
            room_complete = true;
        }
    }
}
