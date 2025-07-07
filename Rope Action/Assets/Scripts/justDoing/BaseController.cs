using UnityEngine;

[RequireComponent(typeof(BaseAttack))]
[RequireComponent(typeof(BaseHP))]
[RequireComponent(typeof(BaseMove))]
abstract public class BaseController : MonoBehaviour
{
    [Header("행동 컴포넌트들")]
    [Tooltip("공격 컴포넌트")]
    protected BaseAttack baseAttack;

    [Tooltip("체력 컴포넌트")]
    protected BaseHP baseHP;

    [Tooltip("이동 컴포넌트")]
    protected BaseMove baseMove;

    virtual protected void Awake()
    {
        TryGetComponent<BaseAttack>(out baseAttack);
        TryGetComponent<BaseHP>(out baseHP);
        TryGetComponent<BaseMove>(out baseMove);
    }
}
