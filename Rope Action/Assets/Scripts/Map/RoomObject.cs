using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [Header("������ ���� ��, ���� �߾��� ��ǥ���� �� �۵���. �ƴϸ� ����ѵ� ��ġ�ɰ���")]
    [SerializeField]
    [Tooltip("�� ��")]
    public GameObject topDoor;
    [SerializeField]
    [Tooltip("�Ʒ� ��")]
    public GameObject botomDoor;
    [SerializeField]
    [Tooltip("�� ��")]
    public GameObject leftDoor;
    [SerializeField]
    [Tooltip("���� ��")]
    public GameObject rightDoor;
}
