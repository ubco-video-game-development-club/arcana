using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Artifact", order = 61)]
public class Artifact : ScriptableObject
{
    [SerializeField] private string title;
    public string Title { get => title; }

    [SerializeField] private string desc;
    public string Desc { get => desc; }

    [SerializeField] private Sprite hudIcon;
    public Sprite HudIcon { get => hudIcon; }

    [SerializeField] private float damageBonus;
    public float DamageBonus { get => damageBonus; }

    [SerializeField] private float reachBonus;
    public float ReachBonus { get => reachBonus; }

    [SerializeField] private float rangeBonus;
    public float RangeBonus { get => rangeBonus; }

    [SerializeField] private float knockbackBonus;
    public float KnockbackBonus { get => knockbackBonus; }
}
