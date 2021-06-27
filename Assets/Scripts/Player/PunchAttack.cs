using UnityEngine;

[CreateAssetMenu(fileName = "Punch Attack", menuName = "Attacks/Punch Attack", order = 51)]
public class PunchAttack : PlayerAttack
{
    [SerializeField] private int damage;
    [SerializeField] private float reach;
    [SerializeField] private float knockback;
    [SerializeField] private float hitboxRadius;
    [SerializeField] private LayerMask enemyLayers;

    public override void Invoke(AttackData data)
    {
        int moddedDamage = damage + data.damageMod;
        float moddedReach = reach + data.reachMod;
        float moddedKnockback = knockback + data.knockbackMod;

        RaycastHit2D hit = Physics2D.CircleCast(data.origin, hitboxRadius, data.direction, moddedReach - hitboxRadius, enemyLayers);
        if (hit && hit.transform.TryGetComponent<Enemy>(out Enemy enemy))
        {
            bool killedTarget = enemy.TakeDamage(moddedDamage);
            if (!killedTarget)
            {
                Vector2 pushForce = data.direction * moddedKnockback;
                enemy.Knockback(pushForce);
            }
        }
    }
}
