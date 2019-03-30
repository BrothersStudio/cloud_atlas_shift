using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChange : MonoBehaviour
{
    public static TimeChange current;

    public Flash flash;
    public Dimension dimension = Dimension.Blue;
    public float last_change_time = 0f;

    List<Enemy> dead_enemies = new List<Enemy>();
    public List<Enemy> enemies = new List<Enemy>();

    public List<Bullet> bullets = new List<Bullet>();

    public List<Wall> walls = new List<Wall>();

    public AudioClip past_to_future;
    public AudioClip future_to_past;
    MusicController music;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        music = FindObjectOfType<MusicController>();
    }

    public void Switch()
    {
        music.SwapMusic();

        last_change_time = Time.timeSinceLevelLoad;

        if (dimension == Dimension.Blue)
        {
            dimension = Dimension.Orange;

            GetComponent<AudioSource>().clip = future_to_past;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            dimension = Dimension.Blue;

            GetComponent<AudioSource>().clip = past_to_future;
            GetComponent<AudioSource>().Play();
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

        foreach (Bullet bullet in bullets)
        {
            if (bullet.gameObject.activeSelf)
            {
                bullet.SwitchDimensions();
            }
        }

        foreach (Wall wall in walls)
        {
            wall.SwitchDimensions();
        }

        Cleanup();
    }

    public void ForceRoomChangeDelay()
    {
        last_change_time = Time.timeSinceLevelLoad;
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