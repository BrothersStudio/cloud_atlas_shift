using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChange : MonoBehaviour
{
    public static TimeChange current;

    public Flash flash;
    public Dimension dimension = Dimension.Blue;

    List<Enemy> dead_enemies = new List<Enemy>();
    public List<Enemy> enemies = new List<Enemy>();

    public List<Wall> walls = new List<Wall>();

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
            if (enemy.health > 0)
            {
                enemy.SwitchDimensions();
            }
            else
            {
                dead_enemies.Add(enemy);
            }
        }

        foreach (Wall wall in walls)
        {
            wall.SwitchDimensions();
        }

        Cleanup();
    }

    void Cleanup()
    {
        foreach (Enemy enemy in dead_enemies)
        {
            enemies.Remove(enemy);
        }
        dead_enemies.Clear();
    }
}

public enum Dimension
{
    Blue,
    Orange
}