using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game manager to do various game-wide things as well as hold references to all singletons.
public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public static WorldGenerator WorldGenerator { get; private set; }

    [SerializeField] private Player player1;
    public static Player Player1 { get => Singleton.player1; }

    [SerializeField] private Player player2;
    public static Player Player2 { get => Singleton.player2; }

    void Awake()
    {
        Singleton = this;
        WorldGenerator = GetComponent<WorldGenerator>();
    }

    public static Player GetOtherPlayer(Player currentPlayer)
    {
        return currentPlayer == Player1 ? Player2 : Player1;
    }
}
