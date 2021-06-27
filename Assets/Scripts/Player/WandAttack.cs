using UnityEngine;

[CreateAssetMenu(fileName = "Wand Attack", menuName = "Attacks/Wand Attack", order = 52)]
public class WandAttack : PlayerAttack
{
    [System.Serializable]
    private struct WandEffect
    {
        public float procWeight;
        public Projectile projectilePrefab;
        public GameObject castEffect;
        public int damage;
        public float range;
        public float knockback;
        public float speed;
    }

    [SerializeField] private WandEffect[] wandEffects;
    [SerializeField] private LayerMask enemyLayer;

    public override void Invoke(AttackData data)
    {
        float probSum = 0f;
        foreach (WandEffect eff in wandEffects)
        {
            probSum += eff.procWeight;
        }

        WandEffect effect = wandEffects[0];
        float threshold = 0f;
        float rand = Random.Range(0, probSum);
        foreach (WandEffect eff in wandEffects)
        {
            threshold += eff.procWeight;
            if (rand < threshold)
            {
                effect = eff;
                break;
            }
        }

        Vector2 spawnPos = data.origin + data.direction * 0.5f;
        Quaternion facingRot = Quaternion.FromToRotation(Vector2.up, data.direction);
        Projectile proj = Instantiate(effect.projectilePrefab, spawnPos, facingRot);
        proj.GetComponent<Rigidbody2D>().AddForce(data.direction * effect.speed, ForceMode2D.Impulse);

        AttackData projData = new AttackData();
        projData.origin = spawnPos;
        projData.direction = data.direction;
        projData.damageMod = effect.damage + data.damageMod;
        projData.rangeMod = effect.range + data.rangeMod;
        projData.knockbackMod = effect.knockback + data.knockbackMod;
        proj.SetAttackData(projData);

        Instantiate(effect.castEffect, spawnPos, facingRot);
    }
}
