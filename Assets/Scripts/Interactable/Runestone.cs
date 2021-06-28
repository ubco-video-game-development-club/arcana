using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Runestone", menuName = "Runestone", order = 62)]
public class Runestone : ScriptableObject
{
    [SerializeField] private string title;
    public string Title { get => title; }

    [SerializeField] private string desc;
    public string Desc { get => desc; }

    [SerializeField] private Sprite inactiveHudIcon;
    public Sprite InactiveHudIcon { get => inactiveHudIcon; }

    [SerializeField] private Sprite activeHudIcon;
    public Sprite ActiveHudIcon { get => activeHudIcon; }

    [SerializeField] private RunestoneType runestoneType;
    public RunestoneType RunestoneType { get => runestoneType; }
}
