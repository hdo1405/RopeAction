using UnityEngine;

public class BaseHP : MonoBehaviour
{
    [Header("ü�� ��ġ")]
    [Tooltip("�ִ� ü��")]
    [SerializeField] protected float maxHP = 100f;

    [Tooltip("���� ü��")]
    [SerializeField] protected float curHP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        FullHeal();
    }

    /// <summary>
    /// ���� ü���� �ִ�ü������
    /// </summary>
    virtual protected void FullHeal()
    {
        if (maxHP > 0) { curHP = maxHP; }
        else { Death(); }
    }

    /// <summary>
    /// ������ ���� �� ȣ��.
    /// </summary>
    /// <param name="damage">Damage���� ���� ���.</param>
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
    /// ���� ü���� 0�̸��϶� ȣ��
    /// </summary>
    virtual protected void Death()
    {

    }
}
