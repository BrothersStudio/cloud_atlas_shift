using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoom : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (FindObjectOfType<LevelController>().current_level == 4)
            {
                FindObjectOfType<MusicController>().SetBossMusic();
            }

            FindObjectOfType<RoomController>().MoveToNextRoom();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
