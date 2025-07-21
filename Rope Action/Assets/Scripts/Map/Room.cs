using System.Collections.Generic;
using UnityEngine;

public class Room
{
    // 방 위치
    public Vector2Int position;
    // 연결된 방들
    public List<Room> connectedRooms = new List<Room>();

    // 시작방, 보스방
    public bool isStart = false;
    public bool isBoss = false;
    public bool isDiscovered = false;

    // 시작방과의 거리(보스방 생성 거리 계산용)
    public int depth;

    [Header("방 오브젝트")]
    // 룸 오브젝트 프래팹을 넣으면 됨, 프래팹에는 roomObject를 넣어야 하고 정보(방의 위아래, 왼쪽오른쪽 방문 정보
    public RoomObject roomObject;
    // 방 문들, 처음에 있지만 나중에 없애버려서 길 열거임
    public List<GameObject> doorObjects = new List<GameObject>();

    public void OpenDoor()
    {
        foreach (GameObject go in doorObjects)
        {
            go.SetActive(false);
        }
    }

}
