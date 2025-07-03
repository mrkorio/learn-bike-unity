using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawn settings")]
    public float initialInterval = 1.5f;
    public float minInterval = 0.3f;
    public float intervalDecreaseRate = 0.05f; // cuanto sube la dificultad cada X segundos
    public float difficultyStepTime = 5f; // cada cuanto sube la dificultad

    [Header("Spawn area")]
    public float spawnZ = 20f;
    public float minX = -3f;
    public float maxX = 3f;
    public float minSeparationX = 1.5f; // separación mínima entre obstáculos

    [Header("Probabilidad de múltiples obstáculos")]
    [Range(0f, 1f)] public float chanceExtraObstacle = 0.3f;
    [Range(0f, 1f)] public float chanceSecondExtra = 0.1f; // Probabilidad de que haya un tercer obstaculo en la misma instancia

    float currentInterval;
    float timeSinceLastStep;
    float offSetY;

    void Start()
    {
        currentInterval = initialInterval;
        Invoke(nameof(SpawnLoop), currentInterval);
    }

    void SpawnLoop()
    {
        SpawnObstacles();

        // Dificultad progresiva
        timeSinceLastStep += currentInterval;
        if (timeSinceLastStep >= difficultyStepTime && currentInterval > minInterval)
        {
            currentInterval = Mathf.Max(currentInterval - intervalDecreaseRate, minInterval);
            timeSinceLastStep = 0f;
        }

        // Reprogramar próximo spawn
        Invoke(nameof(SpawnLoop), currentInterval);
    }

    void SpawnObstacles()
    {
        List<float> usedX = new List<float>();

        // Siempre al menos uno
        TrySpawnObstacle(usedX);

        // Chance de segundo
        if (Random.value < chanceExtraObstacle) TrySpawnObstacle(usedX);

        // Chance de tercero
        if (Random.value < chanceSecondExtra) TrySpawnObstacle(usedX);
    }

    void TrySpawnObstacle(List<float> usedX)
    {
        int index = Random.Range(0, obstaclePrefabs.Length);

        for (int attempt = 0; attempt < 10; attempt++)
        {
            float x = Random.Range(minX, maxX);
            if (IsFarFromOthers(x, usedX))
            {
                Vector3 spawnPos = new Vector3(x, offSetY, spawnZ);
                GameObject newObstacle = Instantiate(obstaclePrefabs[index]);
                var scaler = newObstacle.GetComponent<ObstacleScaler>();
                newObstacle.transform.position = new Vector3(x, scaler.offSet, spawnZ);
                newObstacle.transform.rotation = Quaternion.Euler(scaler.rotationX, 0f, 0f);
                usedX.Add(x);
                break;
            }
        }
    }

    bool IsFarFromOthers(float x, List<float> used)
    {
        foreach (float usedX in used)
        {
            if (Mathf.Abs(x - usedX) < minSeparationX)
                return false;
        }
        return true;
    }
}