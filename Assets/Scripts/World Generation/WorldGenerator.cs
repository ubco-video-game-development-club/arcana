using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	public const float CHUNK_DESPAWN_DISTANCE = Chunk.CHUNK_SIZE * CHUNK_SPAWN_RANGE * 2.0f;
    private const int CHUNK_SPAWN_RANGE = 2;
    private const float SPAWN_RADIUS = 5.0f;

    public int Seed { get => seed; }
	public float ForestRadius => poiSpawnRadius + 50.0f;
    private float PerlinOffset => (float)(seed >> 12); //lowest 22 bits of seed

    [SerializeField] private int seed;
    [SerializeField] private bool generateRandomSeedOnStart = false;
    [SerializeField] private float noiseZoom = 16.0f;
    [SerializeField] private float treeRarity = 5.0f;
    [SerializeField] private float treeThreshold = 0.5f;
    [SerializeField] private int maxPoiPerType = 5;
    [SerializeField] private float poiSpawnRadius = 500.0f;
    [SerializeField] private float pathWidth = 3.0f;
    [SerializeField] private float pathDensity = 0.5f;
    [SerializeField] private Chunk chunkPrefab;
    private new Transform camera;
    private List<Chunk> chunks = new List<Chunk>();

    //Paths are just from POI 0 -> POI 1, POI 1 -> POI 2 .. POI n -> POI 0
    private List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();

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

        Random.InitState(seed);
        GeneratePointsOfInterest();
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
        if(position.sqrMagnitude > ForestRadius * ForestRadius) return -1.0f;

		float d = DistanceToNearestPath(position);
        if(d < pathWidth && Random.value * d < pathDensity) return 1.0f;

        if(position.sqrMagnitude < SPAWN_RADIUS * SPAWN_RADIUS) return 0.0f;

        Vector2 nPos = position / noiseZoom;
        float n = Mathf.PerlinNoise(nPos.x + PerlinOffset, nPos.y + PerlinOffset);
        return n;
    }

    public bool IsTreeHere(float noise)
    {
        if(noise < 0.0f) return true;
        if(noise < treeThreshold || noise == 1.0f) return false;

        float r = Random.value;
        if(r * treeRarity < noise)
        {
            return true;
        }

        return false;
    }

    private void GeneratePointsOfInterest()
    {
        pointsOfInterest.Add(new PointOfInterest(Vector2.zero, PointOfInterestType.SPAWN));

        int poiCount = (int)PointOfInterestType.COUNT;
        
        for(int poiIndex = 0; poiIndex < poiCount; poiIndex++)
        {
            PointOfInterestType currentType = (PointOfInterestType)poiIndex;
            int spawnCount = Random.Range(1, maxPoiPerType);
            for(int i = 0; i < spawnCount; i++)
            {
                Vector2 pos = Random.insideUnitCircle * poiSpawnRadius;
                PointOfInterest poi = new PointOfInterest(pos, currentType);
                pointsOfInterest.Add(poi);
            }
        }
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

    private float DistanceToNearestPath(Vector2 pos)
    {
        float shortest = float.MaxValue;

        for(int i = 0; i < pointsOfInterest.Count; i++)
        {
            PointOfInterest poi1 = pointsOfInterest[i];
            PointOfInterest poi2 = pointsOfInterest[(i + 1) % pointsOfInterest.Count];

            Vector2 p0 = pos;
            Vector2 p1 = poi1.Position;
            Vector2 p2 = poi2.Position;

            float d21X = p2.x - p1.x;
            float d21Y = p2.y - p1.y;
            float d = Mathf.Abs(
                (p2.x - p1.x) * (p1.y - p0.y)
                - (p1.x - p0.x) * (p2.y - p1.y)
            ) / Mathf.Sqrt(
                d21X * d21X
                + d21Y * d21Y
            ); //Distance from pos to the line

            if(d < shortest) shortest = d;
        }

        return shortest;
    }
}
