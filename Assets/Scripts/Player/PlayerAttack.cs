using UnityEngine;

public abstract class PlayerAttack : ScriptableObject
{
    [SerializeField] private float cooldown = 1f;
    public float Cooldown { get => cooldown; }

    public bool Enabled { get; set; }

    public abstract void Invoke(AttackData data);
}
