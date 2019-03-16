using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChange : MonoBehaviour
{
    public static TimeChange current;

    public Flash flash;
    public Dimension dimension = Dimension.Blue;

    void Awake()
    {
        current = this;
    }

    public void Switch()
    {
        if (dimension == Dimension.Blue)
        {
            dimension = Dimension.Orange;
        }
        else
        {
            dimension = Dimension.Blue;
        }
        flash.FlashStart();
    }
}

public enum Dimension
{
    Blue,
    Orange
}