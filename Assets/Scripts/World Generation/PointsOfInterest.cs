using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointOfInterestType
{
    Artifact,
    Chest,
    Door,
    Shrine,
    Runestone,
    COUNT,
    SPAWN
}

public struct PointOfInterest
{
    public Vector2 Position => position;
    public PointOfInterestType Type => type;

    private Vector2 position;
    private PointOfInterestType type;

    public PointOfInterest(Vector2 position, PointOfInterestType type)
    {
        this.position = position;
        this.type = type;
    }
}