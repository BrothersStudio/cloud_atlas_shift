using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextFloor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LevelController lev_con = FindObjectOfType<LevelController>();

            lev_con.current_level++;
            lev_con.ResetLevel();
        }
    }
}
