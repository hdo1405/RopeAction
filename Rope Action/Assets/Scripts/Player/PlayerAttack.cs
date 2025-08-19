using Definition;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : BaseAttack
{
    [SerializeField] protected Transform weaponTransform;
    [SerializeField] protected Collider2D weaponCollider;
    protected PlayerController playerController;

    public void Awake()
    {
        playerController = this.GetComponent<PlayerController>();
    }

    public void Attack()
    {
        if (!CanAttack()) return;
        lastAttackTime = Time.time;

        isSwinging = true;
        attackDir = playerController.FloatMouseDir - 90;
        Debug.Log(attackDir);
        hittedEnemy.Clear();
    }

    private int dir = 1;
    private bool isSwinging = false;
    private float attackDir;

    private HashSet<EnemyHP> hittedEnemy = new HashSet<EnemyHP>();

    private void Update()
    {
        if (isSwinging)
        {
            if (!CanAttack())
            {
                float z = 0;
                if (attackDir > 0 || attackDir < - 180)
                    z = Mathf.Lerp(attackDir - 60, attackDir + 60, (Time.time - lastAttackTime) / attackRate.FinalStat());
                else
                    z = Mathf.Lerp(attackDir + 60, attackDir - 60, (Time.time - lastAttackTime) / attackRate.FinalStat());
                weaponTransform.rotation = Quaternion.Euler(0, 0, z * dir);

                Vector3 center = weaponCollider.bounds.center;
                Vector3 halfExtents = weaponCollider.bounds.extents;

                Collider2D[] hits = Physics2D.OverlapBoxAll(center, halfExtents * 2, weaponCollider.transform.rotation.z);

                foreach (Collider2D hit in hits)
                {
                    if (hit.gameObject != weaponCollider.gameObject && hit.gameObject != this.gameObject)
                    {
                        if (hit.TryGetComponent(out EnemyHP enemy))
                        {
                            if (!hittedEnemy.Contains(enemy))
                            {
                                Debug.Log("Enemy Name: " + enemy.name);
                                hittedEnemy.Add(enemy);
                            }
                        }
                    }
                }

            }
            else
            {
                hittedEnemy.Clear();
                isSwinging = false;
            }
        }
        else
        {
            dir = (int)Mathf.Sign(this.GetComponent<Rigidbody2D>().linearVelocityX);
            weaponTransform.rotation = Quaternion.Euler(0, 0, playerController.FloatMouseDir - 90);
        }
    }
}
