using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    int player_room = 0;
    int num_rooms = 3;
    List<GameObject> spawned_rooms = new List<GameObject>();

    public List<GameObject> available_rooms;
    public GameObject stair_room;

    void Awake()
    {
        for (int i = 0; i < num_rooms; i++)
        {
            GameObject new_room = Instantiate(available_rooms[0]);
            if (i == 0)
            {
                new_room.GetComponent<Room>().Activate();
            }

            // Reposition
            Vector3 new_position = new_room.transform.position;
            new_position.y = i * (new_room.GetComponent<Room>().max_and_min_cam_pos[0].y - new_room.GetComponent<Room>().max_and_min_cam_pos[1].y);
            new_room.transform.position = new_position;

            spawned_rooms.Add(new_room);
        }

        GameObject new_stair_room = Instantiate(stair_room);
        Vector3 stair_position = new_stair_room.transform.position;
        stair_position.y = num_rooms * (new_stair_room.GetComponent<Room>().max_and_min_cam_pos[0].y - new_stair_room.GetComponent<Room>().max_and_min_cam_pos[1].y);
        new_stair_room.transform.position = stair_position;
        spawned_rooms.Add(new_stair_room);

        player_room = 0;
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
