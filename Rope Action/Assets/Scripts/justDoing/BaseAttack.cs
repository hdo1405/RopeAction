using Definition;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    [Header("���� ���� ��ġ")]
    [Tooltip("���ݷ�")]
    [SerializeField] protected float attackPower = 1f;

    [Tooltip("���� ��Ÿ�")]
    [SerializeField] protected float attackRange = 3f;

    [Tooltip("���� �ֱ�")]
    [SerializeField] protected float attackRate = 1f;

    [Tooltip("���ݷ�, ��ü �� ������")]
    [SerializeField] protected Definition.Damage defaultDamage;

    [Header("����� ��")]
    [Tooltip("������ ���� �ð�")]
    protected float lastAttackTime = 0f;

    [property: Tooltip("������ ���� �ð�_�б� ����")]
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
    /// ���� �������� (Time.time - ������ ���ݽð�)�� �����ֱ� �̻��̸� true.
    /// </summary>
    /// <returns>���� �����ϸ� true ��ȯ.</returns>
    virtual protected bool CanAttack()
    {
        return (Time.time - lastAttackTime) >= attackRate;
    }

    /// <summary>
    /// target�� damage���� <- ���߿� Ȥ�� ��������, ������ �������� �ٸ� Damage�������.
    /// </summary>
    /// <param name="target"> �ǰ� ���</param>
    /// <param name="damage"> ���� ������</param>
    virtual public void Attack(BaseHP target, Damage damage)
    {
        if (!CanAttack()) return; // ���� ������

        lastAttackTime = Time.time;
    }
}
