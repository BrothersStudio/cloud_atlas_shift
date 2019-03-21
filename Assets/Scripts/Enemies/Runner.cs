using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public float shift_attack_delay;
    public float speed;

    Vector3[] path = new Vector3[] { };
    int targetIndex;

    Player player;
    Enemy enemy;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        StartCoroutine(RefreshPath());
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator RefreshPath()
    {
        while (true)
        {
            PathRequestManager.RequestPath(transform.position, player.transform.position, OnPathFound);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    void Update()
    {
        if (TimeChange.current.dimension == enemy.enemy_dimension && 
            enemy.health > 0 &&
            Time.timeSinceLevelLoad > shift_attack_delay + TimeChange.current.last_change_time)
        {
            //Vector3 direction = player.transform.position - transform.position;
            //GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * speed * Time.deltaTime;
        }
        else if (TimeChange.current.dimension != enemy.enemy_dimension)
        {
            //Invoke("StopSoon", 0.5f);
        }
    }

    void StopSoon()
    {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
