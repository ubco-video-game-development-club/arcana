using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	public const float CHUNK_DESPAWN_DISTANCE = Chunk.CHUNK_SIZE * CHUNK_SPAWN_RANGE * 2.0f;
    private const int CHUNK_SPAWN_RANGE = 2;

    public int Seed { get => seed; }
    private float PerlinOffset => (float)(seed >> 12); //lowest 22 bits of seed

    [SerializeField] private int seed;
    [SerializeField] private bool generateRandomSeedOnStart = false;
    [SerializeField] private float noiseZoom = 16.0f;
    [SerializeField] private float treeRarity = 5.0f;
    [SerializeField] private float treeThreshold = 0.5f;
    [SerializeField] private Chunk chunkPrefab;
    private new Transform camera;
    private List<Chunk> chunks = new List<Chunk>();

    void Awake()
    {
        camera = Camera.main.transform;
    }

    void Start()
    {
        if(generateRandomSeedOnStart)
        {
            seed = Random.Range(System.Int32.MinValue, System.Int32.MaxValue);
            Debug.Log($"Generated seed {seed}");
        }
    }

    void Update()
    {
        FilterChunks();

        Vector2 chunkPos = camera.position / Chunk.CHUNK_SIZE;
        
        for(int x = -CHUNK_SPAWN_RANGE; x < CHUNK_SPAWN_RANGE; x++)
        {
            for(int y = -CHUNK_SPAWN_RANGE; y < CHUNK_SPAWN_RANGE; y++)
            {
                int cx = Mathf.RoundToInt(chunkPos.x) + x;
                int cy = Mathf.RoundToInt(chunkPos.y) + y;
                if(!IsChunkAt(cx, cy))
                {
                    Vector2 wPos = new Vector2(cx * Chunk.CHUNK_SIZE, cy * Chunk.CHUNK_SIZE);
                    Chunk chunk = Instantiate(chunkPrefab, wPos, Quaternion.identity);
                    chunks.Add(chunk);
                }
            }
        }
    }

    //Gets the perlin noise value at the specified position (world coordinated)
    public float GetNoiseAt(Vector2 position)
    {
        Vector2 nPos = position / noiseZoom;
        float n = Mathf.PerlinNoise(nPos.x + PerlinOffset, nPos.y + PerlinOffset);
        return n;
    }

    public bool IsTreeHere(float noise)
    {
        if(noise < treeThreshold) return false;

        float r = Random.value;
        if(r * treeRarity < noise)
        {
            return true;
        }

        return false;
    }

    //Checks if a chunk is at the specified chunk position
    private bool IsChunkAt(int cx, int cy)
    {
        foreach(Chunk chunk in chunks)
        {
            Vector2 cPos = chunk.transform.position / Chunk.CHUNK_SIZE;
            int x = Mathf.RoundToInt(cPos.x);
            int y = Mathf.RoundToInt(cPos.y);
            if(x == cx && y == cy) return true;
        }

        return false;
    }

    //Removes all destroyed chunks
    private void FilterChunks()
    {
        Chunk[] old = chunks.ToArray();
        foreach(Chunk chunk in old)
        {
            if(chunk == null)
            {
                chunks.Remove(chunk);
            }
        }
    }
}
