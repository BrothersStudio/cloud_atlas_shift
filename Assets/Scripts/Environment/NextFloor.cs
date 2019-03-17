using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextFloor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindObjectOfType<LevelController>().ResetLevel();
        }
    }
}
