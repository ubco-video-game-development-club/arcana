using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int CHUNK_SIZE_CELLS = 16;
    public const float CHUNK_CELL_SIZE = 1.0f;

    [SerializeField] private Sprite[] cellSprites;

    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        WorldGenerator wg = GameManager.WorldGenerator;
        Vector2 chunkPos = transform.position;
        for(int y = 0; y < CHUNK_SIZE_CELLS; y++)
        {
            for(int x = 0; x < CHUNK_SIZE_CELLS; x++)
            {
                Vector2 pos = new Vector2(x * CHUNK_CELL_SIZE, y * CHUNK_CELL_SIZE) + chunkPos;
                float noise = wg.GetNoiseAt(pos);
                int index = Mathf.RoundToInt(noise * (cellSprites.Length - 1));
				Sprite sprite = cellSprites[index];

                GameObject cellGO = new GameObject($"Cell {x}-{y}");
                cellGO.transform.position = pos;

                Cell cell = cellGO.AddComponent<Cell>();
                cell.SetSprite(sprite);
            }
        }
    }
}
