using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game manager to do various game-wide things as well as hold references to all singletons.
public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public static WorldGenerator WorldGenerator { get; private set; }
    public static AStar AStar { get; private set; }

    [SerializeField] private Transform player1;
    public static Transform Player1 { get => Singleton.player1; }

    [SerializeField] private Transform player2;
    public static Transform Player2 { get => Singleton.player2; }

    void Awake()
    {
        Singleton = this;
        WorldGenerator = GetComponent<WorldGenerator>();
        AStar = GetComponent<AStar>();
    }

    public static Transform GetOtherPlayer(Transform currentPlayer)
    {
        return currentPlayer == Player1 ? Player2 : Player1;
    }
}
