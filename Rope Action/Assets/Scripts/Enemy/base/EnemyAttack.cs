using Definition;
using UnityEngine;

public class EnemyAttack : BaseAttack
{
    

    override public void Attack(BaseHP target, Damage damage)
    {
        base.Attack(target, damage);

        target.Damage(damage);
    }
}
