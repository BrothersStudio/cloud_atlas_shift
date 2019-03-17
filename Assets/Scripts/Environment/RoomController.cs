using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<GameObject> available_rooms;

    void Awake()
    {
        GameObject new_room = Instantiate(available_rooms[0]);
        new_room.GetComponent<Room>().Activate();

        Camera.main.GetComponent<CameraFollow>().SetMaxMinCameraPosForRoom(new_room.GetComponent<Room>().max_and_min_cam_pos, new_room.transform);
    }
}
