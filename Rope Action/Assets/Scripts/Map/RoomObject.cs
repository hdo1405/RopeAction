using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [Header("프래팹 넣을 때, 방의 중앙이 좌표여야 잘 작동됨. 아니면 요상한데 배치될거임")]
    [SerializeField]
    [Tooltip("윗 문")]
    public GameObject topDoor;
    [SerializeField]
    [Tooltip("아랫 문")]
    public GameObject botomDoor;
    [SerializeField]
    [Tooltip("왼 문")]
    public GameObject leftDoor;
    [SerializeField]
    [Tooltip("오른 문")]
    public GameObject rightDoor;
}
