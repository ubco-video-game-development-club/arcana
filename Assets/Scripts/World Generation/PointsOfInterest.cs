using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointOfInterestType
{
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

    public static int GetSpawnCount(PointOfInterestType type)
    {
        switch(type)
        {
            case PointOfInterestType.Chest:
                return Random.Range(5, 10);
            case PointOfInterestType.Door:
                return Random.Range(5, 10);
            case PointOfInterestType.Shrine:
                return Random.Range(5, 10);
            case PointOfInterestType.Runestone:
                return 4;
            default:
                return -1;
        }
    }
}