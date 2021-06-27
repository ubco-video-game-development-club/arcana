using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AttackData attackData;

    void Update()
    {
        float sqrDist = ((Vector2)transform.position - attackData.origin).sqrMagnitude;
        float sqrRange = attackData.rangeMod * attackData.rangeMod;
        if (sqrDist > sqrRange)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Enemy>(out Enemy enemy))
        {
            bool killedTarget = enemy.TakeDamage(attackData.damageMod);
            if (!killedTarget)
            {
                enemy.Knockback(attackData.direction * attackData.knockbackMod);
            }
            Destroy(gameObject);
        }
    }

    public void SetAttackData(AttackData data) => attackData = data;
}
