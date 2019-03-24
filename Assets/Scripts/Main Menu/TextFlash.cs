using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TextFlash : MonoBehaviour
{
    bool starting_soon = false;
    float last_switch = 0;
    float on_time = 1f;
    float off_time = 0.5f;

    void Update()
    {
        if (Time.timeSinceLevelLoad > last_switch + on_time && GetComponent<Text>().enabled)
        {
            last_switch = Time.timeSinceLevelLoad;
            GetComponent<Text>().enabled = false;
        }
        else if (Time.timeSinceLevelLoad > last_switch + off_time && !GetComponent<Text>().enabled)
        {
            last_switch = Time.timeSinceLevelLoad;
            GetComponent<Text>().enabled = true;
        }

        if (Input.anyKey && !starting_soon)
        {
            on_time /= 2f;
            off_time /= 2f;

            starting_soon = true;
            Invoke("StartSoon", 2f);
        }
    }

    void StartSoon()
    {
        SceneManager.LoadScene("Main");
    }
}
