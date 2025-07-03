using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [Header("Tramo de carretera")]
    public GameObject roadTilePrefab;
    public int initialTiles = 15;
    public float tileLength = 100f;
    public Transform player;

    [Header("Árboles")]
    public GameObject[] treePrefabs;      // Array con prefabs de árboles
    public int minTreesPerTile = 3;       // Cantidad mínima de árboles por tramo
    public int maxTreesPerTile = 6;       // Máxima cantidad de árboles por tramo
    public float treeSpawnOffsetX = 10f;  // Distancia a los costados de la carretera
    public float treeSpawnMinZOffset = 5f;  // Desde qué punto del tile empieza a aparecer árbol
    public float treeSpawnMaxZOffset = 95f; // Hasta qué punto del tile puede aparecer árbol

    private float nextSpawnZ = 0f;
    private Queue<GameObject> roadTiles = new Queue<GameObject>();

    void Start()
    {
        if (roadTilePrefab == null)
        {
            Debug.LogError("Prefab de carretera no asignado.");
            enabled = false;
            return;
        }
        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogWarning("No se asignaron prefabs de árboles, no se generarán árboles.");
        }

        for (int i = 0; i < initialTiles; i++)
        {
            SpawnRoadTile();
        }
    }

    void Update()
    {
        if (roadTiles.Count > 0)
        {
            GameObject oldestTile = roadTiles.Peek();

            if (oldestTile.transform.position.z < player.position.z - tileLength)
            {
                Destroy(roadTiles.Dequeue());
                SpawnRoadTile();
            }
        }
    }

    void SpawnRoadTile()
    {
        float spawnZ = 0f;

        if (roadTiles.Count > 0)
        {
            GameObject lastTile = roadTiles.ToArray()[roadTiles.Count - 1];
            spawnZ = lastTile.transform.position.z + tileLength;
        }

        Vector3 spawnPos = new Vector3(0f, 0f, spawnZ);
        GameObject tile = Instantiate(roadTilePrefab, spawnPos, Quaternion.identity);
        tile.name = $"RoadTile_Z{spawnZ:F0}";
        roadTiles.Enqueue(tile);

        SpawnTreesOnTile(tile);
    }

    void SpawnTreesOnTile(GameObject tile)
    {
        if (treePrefabs == null || treePrefabs.Length == 0) return;

        int treesCount = Random.Range(minTreesPerTile, maxTreesPerTile + 1);

        List<Vector3> usedPositions = new List<Vector3>();
        float minDistanceBetweenTrees = 3f;
        int attemptsLimit = 20;

        for (int i = 0; i < treesCount; i++)
        {

            GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

            Vector3 candidatePos;
            bool isLeftSide;
            int attempts = 0;

            do
            {
                float zPos = tile.transform.position.z + Random.Range(treeSpawnMinZOffset, treeSpawnMaxZOffset);

                //Spawnear un arbol a la izquierda o derecha, según el random
                float xBase = (Random.value < 0.5f) ? -treeSpawnOffsetX : treeSpawnOffsetX;
                float xVariation = Random.Range(-10f, 10f); //Variar la posición en X, para que no queden todos los árboles alineados
                float xPos = xBase + xVariation;
                isLeftSide = xBase < 0;
                candidatePos = new Vector3(xPos, 0f, zPos);

                attempts++;
                if (attempts > attemptsLimit) break;

            } while (IsPositionTooClose(candidatePos, usedPositions, minDistanceBetweenTrees));

            usedPositions.Add(candidatePos);

            GameObject tree = Instantiate(treePrefab, candidatePos, Quaternion.identity);
            tree.transform.parent = tile.transform;

            var treeOffset = tree.GetComponent<Tree>().offSet;
            // Alinear el árbol al suelo
            SpriteRenderer spriteRenderer = tree.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Bounds bounds = spriteRenderer.bounds;

                //Calculamos cuánto hay desde el centro del sprite hasta su base
                float bottomY = bounds.center.y - bounds.extents.y;

                // Cuánto tenemos que bajar para que toque el suelo
                float offsetToGround = bottomY;

                tree.transform.position -= new Vector3(0f, offsetToGround + treeOffset, 0f);

                //Rotar visualmente el sprite para que quede mirando al jugador
                float yRotation = isLeftSide ? -40f : 40f;
                tree.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            }
        }
    }

    bool IsPositionTooClose(Vector3 pos, List<Vector3> others, float minDist)
    {
        foreach (Vector3 otherPos in others)
        {
            if (Vector3.Distance(pos, otherPos) < minDist)
                return true;
        }
        return false;
    }
}