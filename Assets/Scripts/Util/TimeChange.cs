using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChange : MonoBehaviour
{
    public static TimeChange current;

    public Flash flash;
    public Dimension dimension = Dimension.Blue;

    public List<Enemy> enemies = new List<Enemy>();

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

        foreach (Enemy enemy in enemies)
        {
            enemy.SwitchDimensions();
        }
    }
}

public enum Dimension
{
    Blue,
    Orange
}