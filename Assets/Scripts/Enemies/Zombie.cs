using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed;

    Vector3[] path = new Vector3[] { };
    int targetIndex;
    Vector3 target_location;

    bool death_playing = false;
    public AudioClip death_clip;

    int current_ind;
    public List<Sprite> sprites;

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
        SelectTarget();
        StartCoroutine(RefreshPath());

        current_ind = Random.Range(0, sprites.Count);
        StartCoroutine(WalkCycle());
    }

    void Update()
    {
        SetSprite();

        if (enemy.health <= 0 &&
            !death_playing)
        {
            death_playing = true;
            GetComponent<AudioSource>().clip = death_clip;
            GetComponent<AudioSource>().Play();
        }
    }

    void SetSprite()
    {
        if (enemy.health > 0)
        {
            Vector3 check = transform.position - target_location;
            if (check.x > 0)
            {
                sprite_renderer.flipX = true;
            }
            else
            {
                sprite_renderer.flipX = false;
            }
        }

        if (!enemy.flashing)
        {
            sprite_renderer.sprite = sprites[current_ind];
        }
        else
        {
            sprite_renderer.sprite = enemy.flash_sprite;
        }
    }

    IEnumerator WalkCycle()
    {
        while (true)
        {
            current_ind++;
            if (current_ind == sprites.Count)
            {
                current_ind = 0;
            }
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
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

    void SelectTarget()
    {
        while (true)
        {
            target_location = enemy.GetRandomPositionInRoom();
            if (FindObjectOfType<Grid>().NodeFromWorldPoint(target_location).walkable)
            {
                return;
            }
        }
    }
    
    IEnumerator RefreshPath()
    {
        while (enemy.health > 0)
        {
            PathRequestManager.RequestPath(transform.position, target_location, OnPathFound);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (enemy.health > 0)
            {
                if (Hitstop.current.Hitstopped)
                {
                    yield return null;
                    continue;
                }

                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        SelectTarget();
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
        else if (enemy.health > 0)
        {
            PathRequestManager.RequestPath(transform.position, target_location, OnPathFound);
            transform.position = Vector3.MoveTowards(transform.position, target_location, speed * Time.deltaTime);
            yield return null;
        }
        else
        {
            yield break;
        }
    }
}
