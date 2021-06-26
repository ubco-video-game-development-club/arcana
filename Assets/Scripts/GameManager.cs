using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game manager to do various game-wide things as well as hold references to all singletons.
public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public static WorldGenerator WorldGenerator { get; private set; }

    void Awake()
    {
        Singleton = this;
        WorldGenerator = GetComponent<WorldGenerator>();
    }
}
