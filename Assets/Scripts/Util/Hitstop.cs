using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstop : MonoBehaviour
{
    public static Hitstop current;

    public bool Hitstopped = false;
    float hitstop_done = 0;

    void Awake()
    {
        current = this;
    }

    public void FreezeFor(float seconds)
    {
        Hitstopped = true;
        hitstop_done = seconds + Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > hitstop_done)
        {
            Hitstopped = false;
        }
    }
}
