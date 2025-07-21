using System.Collections.Generic;
using UnityEngine;

public class CreatingMap : MonoBehaviour
{
    [Header("방 생성 규칙")]
    [SerializeField]
    [Tooltip("생성할 방 갯수(시작방, 보스방 포함)")]
    private int roomMaxCount = 20;
    [SerializeField]
    [Tooltip("생성할 방들의 프래팹들 0: 시작 방, 1: 보스 방, 2: 연결 방들")]
    private List<GameObject> roomObjects; // 0: 시작 방, 1: 보스 방, 2: 연결 방들
    [SerializeField]
    [Tooltip("길 오브젝트, 가로로 가는 길 넣어주면 됨")]
    private GameObject pathObject;
    [SerializeField]
    [Tooltip("방 크기, 방 오브젝트(현재 정사각형 기준임)의 가로 길이(벽 포함) + 통로 오브젝트의 가로 길이 포함한 값을 넣어주면 됨")]
    private float roomSize = 1.5f;
    [SerializeField]
    [Tooltip("생성할 방들의 프래팹들 0: 시작 방, 1: 보스 방, 2: 연결 방들")]
    private float sideRoomProbability = 60f;

    private List<Room> rooms = new List<Room>();
    private List<Vector2Int> roomPoss = new List<Vector2Int>();

    private int roomCount = 0;
    private bool createStart = false;
    private bool createBoss = false;
    [SerializeField] private int bossLength = 6;

    private void Start()
    {
        RoomGeneratePathFirst();

        // 통로 생성
        foreach (var room in rooms)
        {
            // 연결 되어 있는 방 확인
            foreach (var nbr in room.connectedRooms)
            {
                // 중복 생성 안함, 아랫 쪽 또는 왼 쪽 방엔 길을 안놓음, 오른 쪽과 윗 쪽에만 길을 놓을거임
                if (room.position.x > nbr.position.x || room.position.y > nbr.position.y)
                    continue;

                // 연결할 방들의 중앙 좌표에 길 생성
                Vector3 mid = new Vector3(
                    (room.position.x + nbr.position.x) * 0.5f * roomSize,
                    (room.position.y + nbr.position.y) * 0.5f * roomSize,
                    0);

                // 높이가 같으면 좌우로 이어지는 통로
                bool horizontal = room.position.y == nbr.position.y;
                // 좌우로이면 그대로 생성, 위아래이면 90도 회전
                Quaternion rot = horizontal ? Quaternion.identity : Quaternion.Euler(0, 0, 90);

                // 다리 생성
                Instantiate(pathObject, mid, rot);
            }
        }

        // 방 생성
        foreach (Room room in rooms)
        {
            Vector3 pos = new Vector3(room.position.x * roomSize, room.position.y * roomSize, 0);

            GameObject roomObject;

            // 시작방, 보스방, 방 생성
            if (room.isStart)
                // 시작 방 생성
                roomObject = Instantiate(roomObjects[0], pos, Quaternion.identity);
            else if (room.isBoss)
                // 보스 방 생성
                roomObject = Instantiate(roomObjects[1], pos, Quaternion.identity);
            else
                // 일반 방들 중에서 무작위로 생성 시키기
                roomObject = Instantiate(roomObjects[Random.Range(2, roomObjects.Count)], pos, Quaternion.identity);

            RoomDoorCheck(room, roomObject);
        }
    }

    private void RoomGeneratePathFirst()
    {
        rooms.Clear();
        roomPoss.Clear();
        roomCount = 0;
        createStart = false;
        createBoss = false;

        // 시작 방 생성
        Room startRoom = new Room
        {
            position = Vector2Int.zero,
            isStart = true,
            depth = 0
        };
        rooms.Add(startRoom);
        roomPoss.Add(startRoom.position);
        roomCount++;
        createStart = true;

        Room current = startRoom;
        for (int i = 1; i <= bossLength; i++)
        {
            Vector2Int dir = GetRandomAvailableDirection(current.position);
            Vector2Int newPos = current.position + dir;

            Room newRoom = new Room
            {
                position = newPos,
                depth = i
            };
            current.connectedRooms.Add(newRoom);
            newRoom.connectedRooms.Add(current);

            rooms.Add(newRoom);
            roomPoss.Add(newPos);
            roomCount++;

            current = newRoom;

            if (i == bossLength)
            {
                newRoom.isBoss = true;
                createBoss = true;
            }
        }

        // BFS로 나머지 방 확장
        Queue<Room> queue = new Queue<Room>();
        foreach (var r in rooms)
        {
            if (!r.isBoss && !r.isStart)
                queue.Enqueue(r);
        }

        while (queue.Count > 0 && roomCount < roomMaxCount)
        {
            Room room = queue.Dequeue();
            List<Vector2Int> dirs = GetShuffledDirections();

            foreach (Vector2Int dir in dirs)
            {
                if (roomCount >= roomMaxCount) break;

                Vector2Int newPos = room.position + dir;
                if (roomPoss.Contains(newPos)) continue;

                if (Random.Range(0f, 100f) > sideRoomProbability) continue;

                Room newRoom = new Room
                {
                    position = newPos,
                    depth = room.depth + 1
                };

                room.connectedRooms.Add(newRoom);
                newRoom.connectedRooms.Add(room);

                rooms.Add(newRoom);
                roomPoss.Add(newPos);
                roomCount++;

                queue.Enqueue(newRoom);
            }
        }
    }

    private Vector2Int GetRandomAvailableDirection(Vector2Int from)
    {
        List<Vector2Int> dirs = GetShuffledDirections();
        foreach (var dir in dirs)
        {
            Vector2Int target = from + dir;
            if (!roomPoss.Contains(target))
                return dir;
        }
        return Vector2Int.up;
    }

    private List<Vector2Int> GetShuffledDirections()
    {
        List<Vector2Int> dirs = new List<Vector2Int>
        {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };
        Shuffle(dirs);
        return dirs;
    }

    private void Shuffle(List<Vector2Int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2Int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    // 방의 문 열어버리는것

    private void RoomDoorCheck(Room room, GameObject roomObject)
    {
        // 방 문 열기 및 벽 계산
        room.roomObject = roomObject.GetComponent<RoomObject>();
        room.doorObjects.Add(roomObject.GetComponent<RoomObject>().topDoor);
        room.doorObjects.Add(roomObject.GetComponent<RoomObject>().botomDoor);
        room.doorObjects.Add(roomObject.GetComponent<RoomObject>().leftDoor);
        room.doorObjects.Add(roomObject.GetComponent<RoomObject>().rightDoor);

        // 방 방향들
        List<Vector2Int> dirs = new List<Vector2Int>
        {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        // 문으로 취급안할 오브젝트
        List<GameObject> notDoor = new List<GameObject>();

        // 문인 것들은 제외
        foreach (var ConnectRoom in room.connectedRooms)
        {
            if (ConnectRoom.position - room.position == Vector2Int.right)
            {
                notDoor.Add(room.doorObjects[3]);
            }
            else if (ConnectRoom.position - room.position == Vector2Int.left)
            {
                notDoor.Add(room.doorObjects[2]);
            }
            else if (ConnectRoom.position - room.position == Vector2Int.down)
            {
                notDoor.Add(room.doorObjects[1]);
            }
            else if (ConnectRoom.position - room.position == Vector2Int.up)
            {
                notDoor.Add(room.doorObjects[0]);
            }
        }

        room.doorObjects = notDoor;

        room.OpenDoor();
    }

}
