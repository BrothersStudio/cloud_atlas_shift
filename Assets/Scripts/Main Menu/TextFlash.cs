using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TextFlash : MonoBehaviour
{
    float last_switch = 0;
    float on_time = 0.8f;
    float off_time = 0.3f;

    void Update()
    {
        if (Time.timeSinceLevelLoad > last_switch + on_time && GetComponent<SpriteRenderer>().enabled)
        {
            last_switch = Time.timeSinceLevelLoad;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (Time.timeSinceLevelLoad > last_switch + off_time && !GetComponent<SpriteRenderer>().enabled)
        {
            last_switch = Time.timeSinceLevelLoad;
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
