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
    [SerializeField] private MonsterSpawnTable spawnTable;
    [SerializeField] private float monsterSpawnChance = 0.1f;
    private new Transform camera;
    private Queue<Command> commandBuffer = new Queue<Command>();

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

        Vector2 monsterSpawnPosition = Vector2.zero;
        bool spawnMonsters = false;
        bool spawnChecked = false;
        for(int y = 0; y < CHUNK_SIZE_CELLS; y++)
        {
            for(int x = 0; x < CHUNK_SIZE_CELLS; x++)
            {
                Vector2 pos = new Vector2(x * CHUNK_CELL_SIZE, y * CHUNK_CELL_SIZE) + chunkPos;
                float noise = wg.GetNoiseAt(pos, out bool isTree, out bool isPath);
                int index = Mathf.RoundToInt(noise * (cellSprites.Length - 1));
				Sprite sprite = cellSprites[Mathf.Max(index, 0)];

                commandBuffer.Enqueue(new Command() {
                    type = CommandType.SpawnCell,
                    position = pos,
                    cellSprite = sprite,
                });

                if(isPath && !spawnChecked)
                {
                    spawnMonsters = Random.value < monsterSpawnChance;
                    spawnChecked = true;
                    monsterSpawnPosition = pos;
                }

                if(isTree)
                {
                    commandBuffer.Enqueue(new Command() {
                        type = CommandType.SpawnTree,
                        position = pos,
                        cellSprite = null
                    });
                }
            }
        }

        if(spawnMonsters)
        {
            commandBuffer.Enqueue(new Command(){
                type = CommandType.SpawnMonsters,
                position = monsterSpawnPosition,
                cellSprite = null,
            });
        }

        StartCoroutine(ChunkCommands());
    }

    private IEnumerator ChunkCommands()
    {
        YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame();
		while(commandBuffer.Count > 0)
        {
            Command command = commandBuffer.Dequeue();
            switch(command.type)
            {
                case CommandType.SpawnCell:
                    SpawnCell(command.position, command.cellSprite);
                    break;
                case CommandType.SpawnTree:
                    Instantiate(treePrefab, command.position, Quaternion.identity, transform);
                    break;
                case CommandType.SpawnMonsters:
                    SpawnMonsters(command.position);
                    break;
            }

            yield return waitForEndOfFrame;
        }
    }

    private void SpawnMonsters(Vector2 pos)
    {
        MonsterEncounter[] encounters = spawnTable.encounters;
        int index = Random.Range(0, encounters.Length);
        MonsterEncounter encounter = encounters[index];

        switch(encounter.distribution)
        {
            case MonsterDistribution.Even:
                int evenCount = encounter.count / encounter.monsterPrefabs.Length;
                foreach(GameObject monster in encounter.monsterPrefabs)
                {
                    for(int i = 0; i < evenCount; i++)
                    {
                        Instantiate(monster, pos + Random.insideUnitCircle, Quaternion.identity);
                    }
                }
                break;
            case MonsterDistribution.Random:
                for(int i = 0; i < encounter.count; i++)
                {
                    GameObject[] monsters = encounter.monsterPrefabs;
                    index = Random.Range(0, monsters.Length);
                    Instantiate(monsters[index], pos + Random.insideUnitCircle, Quaternion.identity);
                }
                break;
        }
    }

    private void SpawnCell(Vector2 pos, Sprite sprite)
    {
		GameObject cellGO = new GameObject($"Cell ({pos.x}, {pos.y})");
		cellGO.transform.parent = transform;
		cellGO.transform.position = pos;

		Cell cell = cellGO.AddComponent<Cell>();
		cell.SetSprite(sprite);
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

    private enum CommandType
    {
        SpawnCell,
        SpawnTree,
        SpawnMonsters,
    }

   private struct Command
   {
       public CommandType type;
       public Vector2 position;
       public Sprite cellSprite;
   }
}
