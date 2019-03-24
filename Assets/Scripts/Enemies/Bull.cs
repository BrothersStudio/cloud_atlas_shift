using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : MonoBehaviour
{
    bool charging = false;
    float charge_start = 0;
    float charge_delay = 0.5f;
    Vector3 charge_direction;
    Vector3 player_charge_position;
    bool looking_for_wall = false;

    public AudioClip charge_sound;

    float orig_speed;
    public float speed;

    Vector3[] path = new Vector3[] { };
    int targetIndex;

    Player player;
    Enemy enemy;
    SpriteRenderer sprite_renderer;
    Grid pathfinding_grid;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();

        sprite_renderer = GetComponent<SpriteRenderer>();
        pathfinding_grid = FindObjectOfType<Grid>();
    }

    void Start()
    {
        StartCoroutine(RefreshPath());

        orig_speed = speed;
    }

    void Update()
    {
        if (charging &&
            Time.timeSinceLevelLoad > charge_delay + charge_start)
        {
            speed = Mathf.Clamp(speed + 0.03f, 0, 4);

            transform.position = Vector3.MoveTowards(transform.position, charge_direction, speed * speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, player_charge_position) < 1f)
            {
                looking_for_wall = true;
            }
        }

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
                if (!enemy.CanSeePlayer())
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
                }
                else if (!charging)
                {
                    Charge();
                }
                yield return null;
            }
        }
    }

    void Charge()
    {
        StopAllCoroutines();

        charging = true;
        charge_start = Time.timeSinceLevelLoad;

        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = charge_sound;
            GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().Play();
        }

        charge_direction = (transform.position - player.transform.position) * -100;
        player_charge_position = player.transform.position;

        speed = 0;
    }

    void StopCharge()
    {
        charging = false;
        speed = orig_speed;
        StartCoroutine(RefreshPath());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" && looking_for_wall)
        {
            looking_for_wall = false;
            StopCharge();
        }
    }
}
