using UnityEngine;

[RequireComponent(typeof(BaseAttack))]
[RequireComponent(typeof(BaseHP))]
[RequireComponent(typeof(BaseMove))]
abstract public class BaseController : MonoBehaviour
{
    [Header("�ൿ ������Ʈ��")]
    [Tooltip("���� ������Ʈ")]
    protected BaseAttack baseAttack;

    [Tooltip("ü�� ������Ʈ")]
    protected BaseHP baseHP;

    [Tooltip("�̵� ������Ʈ")]
    protected BaseMove baseMove;

    virtual protected void Awake()
    {
        TryGetComponent<BaseAttack>(out baseAttack);
        TryGetComponent<BaseHP>(out baseHP);
        TryGetComponent<BaseMove>(out baseMove);
    }
}
