using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPool : MonoBehaviour
{
    public static PlayerBulletPool current;

    public int num_bullet;
    public GameObject bullet_prefab;
    List<GameObject> bullet_pool = new List<GameObject>();

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
}