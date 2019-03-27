using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPool : MonoBehaviour
{
    public static PlayerBulletPool current;

    int num_bullet = 20;
    public GameObject bullet_prefab;
    List<GameObject> bullet_pool = new List<GameObject>();

    public GameObject bullet_wall_hit_prefab;
    List<GameObject> bullet_wall_hit_pool = new List<GameObject>();

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        for (int i = 0; i < num_bullet; i++)
        {
            GameObject obj = Instantiate(bullet_prefab, transform) as GameObject;
            obj.SetActive(false);
            bullet_pool.Add(obj);
        }

        for (int i = 0; i < num_bullet; i++)
        {
            GameObject obj = Instantiate(bullet_wall_hit_prefab, transform) as GameObject;
            obj.SetActive(false);
            bullet_wall_hit_pool.Add(obj);
        }
    }

    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < bullet_pool.Count; i++)
        {
            if (!bullet_pool[i].activeInHierarchy)
            {
                return bullet_pool[i];
            }
        }

        GameObject obj = Instantiate(bullet_prefab, transform) as GameObject;
        bullet_pool.Add(obj);
        return obj;
    }

    public GameObject GetPooledWallHit()
    {
        for (int i = 0; i < bullet_wall_hit_pool.Count; i++)
        {
            if (!bullet_wall_hit_pool[i].activeInHierarchy)
            {
                return bullet_wall_hit_pool[i];
            }
        }

        GameObject obj = Instantiate(bullet_wall_hit_prefab, transform) as GameObject;
        bullet_wall_hit_pool.Add(obj);
        return obj;
    }
}