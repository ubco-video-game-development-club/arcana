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

    [SerializeField] private Sprite hudIcon;
    public Sprite HudIcon { get => hudIcon; }

    [SerializeField] private RunestoneType runestoneType;
    public RunestoneType RunestoneType { get => runestoneType; }
}
