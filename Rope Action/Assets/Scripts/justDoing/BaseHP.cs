using UnityEngine;

public class BaseHP : MonoBehaviour
{
    [Header("체력 수치")]
    [Tooltip("최대 체력")]
    [SerializeField] protected float maxHP = 100f;

    [Tooltip("현재 체력")]
    [SerializeField] protected float curHP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        FullHeal();
    }

    /// <summary>
    /// 현재 체력을 최대체력으로
    /// </summary>
    virtual protected void FullHeal()
    {
        if (maxHP > 0) { curHP = maxHP; }
        else { Death(); }
    }

    /// <summary>
    /// 데미지 입을 때 호출.
    /// </summary>
    /// <param name="damage">Damage변수 전달 요망.</param>
    virtual public void Damage(Definition.Damage damage)
    {
        if (curHP - damage.AttackPower < 0)
        {
            Death();
            return;
        }

        curHP -= damage.AttackPower;
    }

    /// <summary>
    /// 현재 체력이 0미만일때 호출
    /// </summary>
    virtual protected void Death()
    {

    }
}
