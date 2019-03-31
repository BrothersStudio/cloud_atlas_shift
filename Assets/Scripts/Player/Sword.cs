using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    int damage = 50;

    int last_bullet_swing_id;
    int swing_id = 0;
    int swing_frame_count = 0;
    public BoxCollider2D[] swing_boxes;

    public Sprite[] swing_sprites;

    Vector3 orig_pos;
    Quaternion orig_rot;

    public void Swing()
    {
        swing_id++;

        if (orig_pos == null)
        {
            orig_pos = new Vector3(0, 0.43f, -0.5f);
            orig_rot = transform.rotation;
        }

        SelectAnimation();

        gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().sprite = swing_sprites[0];
        swing_boxes[0].enabled = true;

        swing_frame_count = 0;
    }

    void FixedUpdate()
    {
        if (Hitstop.current.Hitstopped)
        {
            return;
        }

        if (swing_frame_count >= 10)
        {
            gameObject.SetActive(false);
            swing_boxes[2].enabled = false;
        }
        else if (swing_frame_count >= 8)
        {
            GetComponent<SpriteRenderer>().sprite = swing_sprites[2];
            swing_boxes[1].enabled = false;
            swing_boxes[2].enabled = true;
        }
        else if (swing_frame_count >= 2)
        {
            GetComponent<SpriteRenderer>().sprite = swing_sprites[1];
            swing_boxes[0].enabled = false;
            swing_boxes[1].enabled = true;
        }
        swing_frame_count++;
    }

    void SelectAnimation()
    {
        ResetFacing();
        switch (GetComponentInParent<PlayerSprite>().player_direction)
        {
            case WalkDir.Up:
                transform.localPosition = new Vector3(0, 0.82f, 0);
                break;
            case WalkDir.Down:
                GetComponent<SpriteRenderer>().flipY = true;
                GetComponent<SpriteRenderer>().sortingLayerName = "Above Char Weapons";

                transform.localPosition = new Vector3(0, -0.82f, 0);
                break;
            case WalkDir.Left:
                GetComponent<SpriteRenderer>().flipX = true;

                transform.Rotate(new Vector3(0, 0, 90));
                transform.localPosition = new Vector3(-0.82f, 0, 0);
                break;
            case WalkDir.Right:
                GetComponent<SpriteRenderer>().flipX = true;
                GetComponent<SpriteRenderer>().flipY = true;

                transform.Rotate(new Vector3(0, 0, 90));
                transform.localPosition = new Vector3(0.82f, 0, 0);
                break;
        }
    }

    void ResetFacing()
    {
        transform.localPosition = orig_pos;
        transform.rotation = orig_rot;

        GetComponent<SpriteRenderer>().flipY = false;
        GetComponent<SpriteRenderer>().flipX = false;

        GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy.last_swing_id != swing_id && (enemy.enemy_dimension == Dimension.Orange || enemy.is_boss))
            {
                enemy.SwordHit(damage, transform.parent, swing_id);
                Hitstop.current.HitstopFor(0.15f);
            }
        }
        else if (collision.tag == "Bullet" && swing_id != last_bullet_swing_id)
        {
            if (collision.GetComponent<Bullet>().bullet_dimension == TimeChange.current.dimension)
            {
                last_bullet_swing_id = swing_id;

                collision.gameObject.SetActive(false);
                Hitstop.current.HitstopFor(0.1f);
                FindObjectOfType<SwordFlash>().Flash();
            }
        }
    }
}
