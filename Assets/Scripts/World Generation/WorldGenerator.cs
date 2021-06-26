using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int Seed { get => seed; }

    [SerializeField] private int seed;
    [SerializeField] private bool generateRandomSeedOnStart = false;
    [SerializeField] private float noiseZoom = 16.0f;

    void Start()
    {
        if(generateRandomSeedOnStart)
        {
            seed = Random.Range(System.Int32.MinValue, System.Int32.MaxValue);
            Debug.Log($"Generated seed {seed}");
        }
    }

    //Gets the perlin noise value at the specified position (world coordinated)
    public float GetNoiseAt(Vector2 position)
    {
        Vector2 nPos = position / noiseZoom;
        float n = Mathf.PerlinNoise(nPos.x + seed, nPos.y + seed);
        return n;
    }
}
