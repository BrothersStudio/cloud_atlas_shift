using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public float shift_attack_delay;
    public float speed;

    bool animating = true;
    int animation_ind = 0;
    public List<Sprite> animation;

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

        animation_ind = Random.Range(0, animation.Count);
        StartCoroutine(SkullAnimation());
    }

    void Update()
    {
        SetSprite();
    }

    void SetSprite()
    {
        if (TimeChange.current.dimension == enemy.enemy_dimension && 
            enemy.health > 0)
        {
            Vector3 check = transform.position - player.transform.position;
            if (check.x > 0)
            {
                sprite_renderer.flipX = true;
            }
            else
            {
                sprite_renderer.flipX = false;
            }
        }

        if (enemy.flashing)
        {
            StopCoroutine(SkullAnimation());
            sprite_renderer.sprite = enemy.flash_sprite;
            animating = false;
        }
        else if (!animating)
        {
            StartCoroutine(SkullAnimation());
            animating = true;
        }
    }

    IEnumerator SkullAnimation()
    {
        while (enemy.health > 0)
        {
            animation_ind++;
            if (animation_ind == animation.Count)
            {
                animation_ind = 0;
            }
               
            sprite_renderer.sprite = animation[animation_ind];
            yield return new WaitForSeconds(0.2f);
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
            PathRequestManager.RequestPath(transform.position, player.transform.position, OnPathFound);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (TimeChange.current.dimension == enemy.enemy_dimension && enemy.health > 0)
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

    void StopSoon()
    {

    }
}
