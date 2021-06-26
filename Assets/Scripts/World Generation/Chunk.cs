using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int CHUNK_SIZE_CELLS = 16;
    public const float CHUNK_CELL_SIZE = 1.0f;

    void Start()
    {
        byte[] cells = Generate();
    }

    private byte[] Generate()
    {
        byte[] cells = new byte[CHUNK_SIZE_CELLS * CHUNK_SIZE_CELLS];

        Vector2 chunkPos = transform.position;
        for(int y = 0; y < CHUNK_SIZE_CELLS; y++)
        {
            for(int x = 0; x < CHUNK_SIZE_CELLS; x++)
            {
                cells[x + CHUNK_SIZE_CELLS * y] = 0;
            }
        }

        return cells;
    }
}
