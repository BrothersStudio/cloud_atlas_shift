using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed;

    Vector3[] path = new Vector3[] { };
    int targetIndex;

    Player player;
    Enemy enemy;
    SpriteRenderer sprite_renderer;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(RefreshPath());
    }

    void Update()
    {
        SetSprite();
    }

    void SetSprite()
    {
        if (!enemy.flashing)
        {
            sprite_renderer.sprite = enemy.normal_sprite;
        }
        else
        {
            sprite_renderer.sprite = enemy.flash_sprite;
        }
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
        while (enemy.health > 0)
        {
            PathRequestManager.RequestPath(transform.position, GetRandomPositionInRoom(), OnPathFound);

            yield return new WaitForSeconds(10f);
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (enemy.health > 0)
                {
                    if (transform.position == currentWaypoint)
                    {
                        targetIndex++;
                        if (targetIndex >= path.Length)
                        {
                            PathRequestManager.RequestPath(transform.position, GetRandomPositionInRoom(), OnPathFound);
                            yield break;
                        }
                        currentWaypoint = path[targetIndex];
                    }

                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                }
                yield return null;
            }
        }
    }

    Vector3 GetRandomPositionInRoom()
    {
        return new Vector3(
            Random.Range(enemy.min_pos.x, enemy.max_pos.x),
            Random.Range(enemy.min_pos.y, enemy.max_pos.y),
            0);
    }
}
