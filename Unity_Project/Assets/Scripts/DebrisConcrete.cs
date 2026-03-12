using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisConcrete : MonoBehaviour
{
    public GameObject fallingPrefab;

    // Vertical spawning options
    public float spawnYStart = 6.5f;
    public float spawnYStep = 1f;
    public int verticalSteps = 3; // number of heights to choose from

    public float minX = 0.5f;
    public float maxX = 10.5f;

    public int objectsPerWave = 5;

    public float minWaveDelay = 2f;
    public float maxWaveDelay = 4f;

    public float quickSpawnDelay = 0.15f;

    private List<float> gridX = new List<float>();
    private List<float> gridY = new List<float>();

    void Start()
    {
        GenerateGrid();
        StartCoroutine(SpawnLoop());
    }

    void GenerateGrid()
    {
        gridX.Clear();
        gridY.Clear();

        // X positions
        for (float x = minX; x <= maxX; x += 2f) // 2-unit gap
        {
            gridX.Add(x);
        }

        // Y positions
        for (int i = 0; i < verticalSteps; i++)
        {
            gridY.Add(spawnYStart + i * spawnYStep);
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnWave();

            float waitTime = Random.Range(minWaveDelay, maxWaveDelay);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnWave()
    {
        List<Vector2> availablePositions = new List<Vector2>();

        // Make all possible (X,Y) combinations
        foreach (float x in gridX)
        {
            foreach (float y in gridY)
            {
                availablePositions.Add(new Vector2(x, y));
            }
        }

        List<Vector2> chosenPositions = new List<Vector2>();

        // Randomly pick positions until we have enough
        for (int i = 0; i < objectsPerWave && availablePositions.Count > 0; i++)
        {
            int index = Random.Range(0, availablePositions.Count);
            Vector2 chosen = availablePositions[index];
            chosenPositions.Add(chosen);

            // Remove any positions horizontally too close
            availablePositions.RemoveAll(pos => Mathf.Abs(pos.x - chosen.x) < 2f);
        }

        StartCoroutine(SpawnObjects(chosenPositions));
    }

    IEnumerator SpawnObjects(List<Vector2> positions)
    {
        foreach (Vector2 pos in positions)
        {
            Vector3 spawnPos = new Vector3(pos.x, pos.y, 0);
            Instantiate(fallingPrefab, spawnPos, Quaternion.identity);

            if (Random.value > 0.4f)
                yield return new WaitForSeconds(Random.Range(0f, quickSpawnDelay));
        }
    }
}