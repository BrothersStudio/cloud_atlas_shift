using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage = 20;

    public float speed;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Hitstop.current.Hitstopped)
        {
            rigid.simulated = false;
        }
        else if (rigid.simulated == false)
        {
            rigid.simulated = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (collision.GetComponent<Enemy>().enemy_dimension == Dimension.Blue)
            {
                gameObject.SetActive(false);
                collision.GetComponent<Enemy>().Hit(damage);
            }
        }
        else if (collision.tag == "Wall")
        {
            Invoke("Disappear", 0.04f);
        }
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
}
