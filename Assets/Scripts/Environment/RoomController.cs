using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    int player_room = 0;
    int num_rooms = 3;
    int new_room_height_factor = 0;
    List<GameObject> spawned_rooms = new List<GameObject>();

    public List<GameObject> available_rooms;
    public GameObject stair_room;

    List<float> x_vals = new List<float>();
    List<float> y_vals = new List<float>();
    public Grid pathfinding_grid;

    void Awake()
    {
        for (int i = 0; i < num_rooms; i++)
        {
            // TODO: Room selection, somehow
            GameObject new_room = Instantiate(available_rooms[0]);
            Room room_comp = new_room.GetComponent<Room>();
            if (i == 0)
            {
                room_comp.Activate();
            }

            // Reposition
            Vector3 new_position = new_room.transform.position;
            new_position.y = i * (room_comp.max_and_min_cam_pos[0].y - room_comp.max_and_min_cam_pos[1].y);
            new_room.transform.position = new_position;
            new_room_height_factor += (int)(room_comp.max_and_min_cam_pos[0].y - room_comp.max_and_min_cam_pos[1].y);

            Vector3 max_camera_pos = new_room.transform.TransformPoint(room_comp.max_and_min_cam_pos[0]);
            Vector3 min_camera_pos = new_room.transform.TransformPoint(room_comp.max_and_min_cam_pos[1]);

            x_vals.Add(max_camera_pos.x);
            x_vals.Add(min_camera_pos.x);

            y_vals.Add(max_camera_pos.y);
            y_vals.Add(min_camera_pos.y);

            spawned_rooms.Add(new_room);
        }
        // Position stair room
        GameObject new_stair_room = Instantiate(stair_room);
        Vector3 stair_position = new_stair_room.transform.position;
        stair_position.y = new_room_height_factor;
        new_stair_room.transform.position = stair_position;
        spawned_rooms.Add(new_stair_room);

        player_room = 0;

        // Position pathfinding grid
        float x_dist = Mathf.Max(x_vals.ToArray()) - Mathf.Min(x_vals.ToArray());
        float y_dist = Mathf.Max(y_vals.ToArray()) - Mathf.Min(y_vals.ToArray());
        pathfinding_grid.transform.position = new Vector3(x_vals.Average(), y_vals.Average(), 0);
        pathfinding_grid.gridWorldSize = new Vector2(x_dist, y_dist);
    }

    public void MoveToNextRoom()
    {
        player_room++;

        TimeChange.current.ForceRoomChangeDelay();

        spawned_rooms[player_room].GetComponent<Room>().Activate();

        Player player = FindObjectOfType<Player>();
        Vector3 player_pos = spawned_rooms[player_room].transform.position;
        player_pos.y -= 3;
        player.transform.position = player_pos;

        Camera.main.GetComponent<CameraFollow>().MoveToNextRoom(spawned_rooms[player_room].transform.position);
    }
}
