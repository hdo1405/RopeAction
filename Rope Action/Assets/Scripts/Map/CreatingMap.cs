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
    private List<GameObject> StartRoomObjects; // 0: 시작 방, 1: 보스 방, 2: 연결 방들
    [Tooltip("생성할 방들의 프래팹들 0: 시작 방, 1: 보스 방, 2: 연결 방들")]
    private List<GameObject> BossRoomObjects; // 0: 시작 방, 1: 보스 방, 2: 연결 방들
    [Tooltip("생성할 방들의 프래팹들 0: 시작 방, 1: 보스 방, 2: 연결 방들")]
    private List<GameObject> NormalRoomObjects; // 0: 시작 방, 1: 보스 방, 2: 연결 방들
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
        // 방 머리 속으로만 만들기
        RoomGeneratePathFirst();

        //// 리스트로만 만든 맵을 오브젝트로 생성하는 것
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
                roomObject = Instantiate(StartRoomObjects[Random.Range(0, StartRoomObjects.Count)], pos, Quaternion.identity);
            else if (room.isBoss)
                // 보스 방 생성
                roomObject = Instantiate(BossRoomObjects[Random.Range(0, BossRoomObjects.Count)], pos, Quaternion.identity);
            else
                // 일반 방들 중에서 무작위로 생성 시키기
                roomObject = Instantiate(NormalRoomObjects[Random.Range(0, NormalRoomObjects.Count)], pos, Quaternion.identity);

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

        // 보스 방 까지 가는 길 생성
        Room current = startRoom;
        // 보스 길 까지 반복
        for (int i = 1; i <= bossLength; i++)
        {
            // 시작방하고 보스방들 중에서 무작위로 가져오고
            // 연결가능한거 확인, 보스 방은 연결가능한 길로 나와야함

            // 가능한 방향 무작위로 가져오고, 위치 저장
            Vector2Int dir = GetRandomAvailableDirection(current.position);
            Vector2Int newPos = current.position + dir;

            // 새 방 생성 및 리스트에 방과 위치 저장
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

            // 만약 보스 생성 위치와 거리가 같은 경우, 보스 방으로 생성
            if (i == bossLength)
            {
                newRoom.isBoss = true;
                createBoss = true;
            }
        }

        // BFS로 나머지 방 확장
        Queue<Room> queue = new Queue<Room>();
        // 대강 방 추가 되면 나중에 확일할 리스트에 추가 되고 빙빙 돌려가는거
        foreach (var r in rooms)
        {
            // 시작방과 보스방이 아니면 넣는다. 아직도 왜 GPT가 넣은지 모르는거
            //if (!r.isBoss && !r.isStart)
                queue.Enqueue(r);
        }

        // 최대 생성 갯수 만들 때 까지 반복
        while (queue.Count > 0 && roomCount < roomMaxCount)
        {
            // 추가로 생성된거, 확장할 리스트에 추가
            Room room = queue.Dequeue();
            // 랜덤 방향 섞기, 굳이 인거 같긴한데 일단 놔둠
            List<Vector2Int> dirs = GetShuffledDirections();

            foreach (Vector2Int dir in dirs)
            {
                // 방 갯수 초과 확인
                if (roomCount >= roomMaxCount) break;

                // 생성할 방 위치
                Vector2Int newPos = room.position + dir;
                // 중복 체크
                if (roomPoss.Contains(newPos)) continue;
                // 마지막 방이 아니거나, 확률이 나오지 않은 경우 다음방 확인
                if (queue.Count != 0 && Random.Range(0f, 100f) > sideRoomProbability) continue;

                // 새방 생성
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

    // 방향중에 남은 방향 가져오기
    private Vector2Int GetRandomAvailableDirection(Vector2Int from)
    {
        // 무작위로 선택되게 섞기
        List<Vector2Int> dirs = GetShuffledDirections();
        foreach (var dir in dirs)
        {
            Vector2Int target = from + dir; 
            // 남은 자리 있으면 리턴
            if (!roomPoss.Contains(target))
                return dir;
        }
        return Vector2Int.up;
    }

    // 방향 생성 후 섞기
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

    // 방향 섞기
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

    // 방의 문,벽 확인하기
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
