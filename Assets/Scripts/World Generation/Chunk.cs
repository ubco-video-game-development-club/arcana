using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int CHUNK_SIZE_CELLS = 16;
    public const float CHUNK_CELL_SIZE = 1.0f;
    public const float CHUNK_SIZE = CHUNK_SIZE_CELLS * CHUNK_CELL_SIZE;
    public const int PSEED_PRIME = 816_887_069;


	[SerializeField] private Sprite[] cellSprites;
    [SerializeField] private GameObject treePrefab;
    private Transform camera;

    void Awake()
    {
        camera = Camera.main.transform;
    }

    void Start()
    {
        Generate();
    }

    void Update()
    {
        Vector2 d = camera.position - transform.position;
        if(d.sqrMagnitude > WorldGenerator.CHUNK_DESPAWN_DISTANCE * WorldGenerator.CHUNK_DESPAWN_DISTANCE)
        {
            Destroy(gameObject);
        }
    }

    private void Generate()
    {
		WorldGenerator wg = GameManager.WorldGenerator;
		Vector2 chunkPos = transform.position;
        int pSeed = GeneratePositionalSeed(chunkPos) + wg.Seed;
        Random.InitState(pSeed);

        for(int y = 0; y < CHUNK_SIZE_CELLS; y++)
        {
            for(int x = 0; x < CHUNK_SIZE_CELLS; x++)
            {
                Vector2 pos = new Vector2(x * CHUNK_CELL_SIZE, y * CHUNK_CELL_SIZE) + chunkPos;
                float noise = wg.GetNoiseAt(pos);
                int index = Mathf.RoundToInt(noise * (cellSprites.Length - 1));
				Sprite sprite = cellSprites[index];
                
                GameObject cellGO = new GameObject($"Cell {x}-{y}");
                cellGO.transform.parent = transform;
                cellGO.transform.position = pos;

                Cell cell = cellGO.AddComponent<Cell>();
                cell.SetSprite(sprite);

                if(wg.IsTreeHere(noise))
                {
                    Instantiate(treePrefab, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    //This is to generate "random" seeds based on position.
    private int GeneratePositionalSeed(Vector2 position)
    {
        float x = position.x;
        float y = position.y;

        int wholeX = (int)x;
        int wholeY = (int)y;

        int seed = wholeX ^ wholeY * (wholeX + wholeY) ^ PSEED_PRIME;
        return seed;
    }
}
