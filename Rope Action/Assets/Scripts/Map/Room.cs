using System.Collections.Generic;
using UnityEngine;

public class Room
{
    // �� ��ġ
    public Vector2Int position;
    // ����� ���
    public List<Room> connectedRooms = new List<Room>();

    // ���۹�, ������
    public bool isStart = false;
    public bool isBoss = false;
    public bool isDiscovered = false;

    // ���۹���� �Ÿ�(������ ���� �Ÿ� ����)
    public int depth;

    [Header("�� ������Ʈ")]
    // �� ������Ʈ �������� ������ ��, �����տ��� roomObject�� �־�� �ϰ� ����(���� ���Ʒ�, ���ʿ����� �湮 ����
    public RoomObject roomObject;
    // �� ����, ó���� ������ ���߿� ���ֹ����� �� ������
    public List<GameObject> doorObjects = new List<GameObject>();

    public void OpenDoor()
    {
        foreach (GameObject go in doorObjects)
        {
            go.SetActive(false);
        }
    }

}
