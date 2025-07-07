using Definition;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    [Header("공격 관련 수치")]
    [Tooltip("공격력")]
    [SerializeField] protected float attackPower = 1f;

    [Tooltip("공격 사거리")]
    [SerializeField] protected float attackRange = 3f;

    [Tooltip("공격 주기")]
    [SerializeField] protected float attackRate = 1f;

    [Tooltip("공격력, 주체 등 데이터")]
    [SerializeField] protected Definition.Damage defaultDamage;

    [Header("디버깅 용")]
    [Tooltip("마지막 공격 시간")]
    protected float lastAttackTime = 0f;

    [property: Tooltip("마지막 공격 시간_읽기 전용")]
    public float LastAttackTime
    {
        get { return lastAttackTime; }
        protected set { lastAttackTime = value; }
    }

    virtual protected void Start()
    {
        defaultDamage = new Definition.Damage(attackPower, this.gameObject);
    }

    /// <summary>
    /// 공격 가능한지 (Time.time - 마지막 공격시간)이 공격주기 이상이면 true.
    /// </summary>
    /// <returns>공격 가능하면 true 반환.</returns>
    virtual protected bool CanAttack()
    {
        return (Time.time - lastAttackTime) >= attackRate;
    }

    /// <summary>
    /// target과 damage받음 <- 나중에 혹시 공격패턴, 데미지 여러개면 다른 Damage넣으라고.
    /// </summary>
    /// <param name="target"> 피격 대상</param>
    /// <param name="damage"> 공격 데미지</param>
    virtual public void Attack(BaseHP target, Damage damage)
    {
        if (!CanAttack()) return; // 아직 안지남

        lastAttackTime = Time.time;
    }
}
