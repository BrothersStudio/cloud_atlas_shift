using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextFloor : MonoBehaviour
{
    bool waiting = false;

    FadeToBlack fade;
    LevelController lev_con;

    void Awake()
    {
        fade = FindObjectOfType<FadeToBlack>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            waiting = true;
            fade.FadeOut();

            GetComponent<AudioSource>().Play();

            Hitstop.current.HitstopFor(2);

            lev_con = FindObjectOfType<LevelController>();
        }
    }

    void Update()
    {
        if (fade.done && waiting)
        {
            waiting = false;
            lev_con.current_level++;
            lev_con.ResetLevel();
        }
    }
}
