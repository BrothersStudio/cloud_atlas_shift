using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool stair_room;

    public List<Direction> exits;
    public List<Vector3> exit_points;

    public List<GameObject> walls;
    public List<GameObject> doors;
    public List<Door> doors_to_open = new List<Door>();

    public List<GameObject> enemy_to_place;
    public List<Vector3> location_to_place;

    List<Enemy> enemies = new List<Enemy>();
    bool room_complete = false;

    public List<Vector3> max_and_min_cam_pos;

    Grid pathfinding_grid;

    void Awake()
    {
        pathfinding_grid = FindObjectOfType<Grid>();
    }

    public void Activate()
    {
        Vector3 global_max_pos = transform.TransformPoint(GetComponent<Room>().max_and_min_cam_pos[0]);
        Vector3 global_min_pos = transform.TransformPoint(GetComponent<Room>().max_and_min_cam_pos[1]);

        for (int i = 0; i < enemy_to_place.Count; i++)
        {
            GameObject new_enemy = Instantiate(enemy_to_place[i], transform.TransformPoint(location_to_place[i]), Quaternion.identity);
            new_enemy.GetComponent<Enemy>().SetRoomPosition(global_max_pos, global_min_pos);
            enemies.Add(new_enemy.GetComponent<Enemy>());
        }

        Camera.main.GetComponent<CameraFollow>().SetMaxMinCameraPosForRoom(global_max_pos, global_min_pos);

        AddDoors();

        room_complete = false;

        pathfinding_grid.CreateGrid();
    }

    void AddDoors()
    {
        // Spawn listed doors
        List<int> exit_inds = new List<int>();
        foreach (Direction exit in exits)
        {
            int ind = 0;
            switch (exit)
            {
                case Direction.North:
                    ind = 0;
                    break;
                case Direction.South:
                    ind = 1;
                    break;
                case Direction.East:
                    ind = 2;
                    break;
                case Direction.West:
                    ind = 3;
                    break;
            }
            exit_inds.Add(ind);

            GameObject new_door = Instantiate(doors[ind], transform.TransformPoint(exit_points[ind]), Quaternion.identity);
            if (ind != 1)
            {
                doors_to_open.Add(new_door.GetComponent<Door>());
            }
        }

        // Spawn walls in leftover slots
        for (int i = 0; i < exit_points.Count; i++)
        {
            if (exit_inds.Contains(i)) continue;

            if (stair_room && i == 0) continue;

            Instantiate(walls[i], transform.TransformPoint(exit_points[i]), Quaternion.identity);
        }
    }

    void Update()
    {
        if (!room_complete)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.health > 0 && !enemy.is_hazard)
                {
                    return;
                }
            }

            CompleteRoom();

            OpenDoors();
        }
    }

    void CompleteRoom()
    {
        room_complete = true;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.is_hazard)
            {
                enemy.health = -10;
            }
        }
    }

    void OpenDoors()
    {
        foreach (Door door in doors_to_open)
        {
            door.Open();
        }
    }
}

public enum Direction
{
    North,
    South,
    East,
    West
}
