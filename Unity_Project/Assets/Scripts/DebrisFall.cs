using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisFall : MonoBehaviour
{
    public GameObject fallingPrefab;

    // Vertical spawning options
    public float spawnYStart = 6.5f;
    public float spawnYStep = 1f;
    public int verticalSteps = 3;

    public float minX = 0.5f;
    public float maxX = 10.5f;

    [Header("Objects Per Wave Range")]
    public int minObjectsPerWave = 1;
    public int maxObjectsPerWave = 2;

    public float quickSpawnDelay = 0.15f;

    [Header("Angular / Movement Settings")]
    public bool useAngularVelocity = true;
    public float minAngularVelocity = -200f;
    public float maxAngularVelocity = 200f;

    public bool useHorizontalForce = true;
    public float minHorizontalForce = -2f;
    public float maxHorizontalForce = 2f;

    [Header("Directional Control")]
    public float centerX = 5.5f; // Middle of play area

    private List<float> gridX = new List<float>();
    private List<float> gridY = new List<float>();

    void GenerateGrid()
    {
        gridX.Clear();
        gridY.Clear();

        for (float x = minX; x <= maxX; x += 0.25f)
        {
            gridX.Add(x);
        }

        for (int i = 0; i < verticalSteps; i++)
        {
            gridY.Add(spawnYStart + i * spawnYStep);
        }
    }

    public void SpawnWave()
    {
        GenerateGrid();

        List<Vector2> availablePositions = new List<Vector2>();

        foreach (float x in gridX)
        {
            foreach (float y in gridY)
            {
                availablePositions.Add(new Vector2(x, y));
            }
        }

        // Pick random number of objects for this wave
        int objectsThisWave = Random.Range(minObjectsPerWave, maxObjectsPerWave + 1);

        List<Vector2> chosenPositions = new List<Vector2>();

        for (int i = 0; i < objectsThisWave && availablePositions.Count > 0; i++)
        {
            int index = Random.Range(0, availablePositions.Count);
            Vector2 chosen = availablePositions[index];
            chosenPositions.Add(chosen);

            // Enforce at least 1 unit horizontal spacing
            availablePositions.RemoveAll(pos => Mathf.Abs(pos.x - chosen.x) < 1f);
        }

        StartCoroutine(SpawnObjects(chosenPositions));
    }

    IEnumerator SpawnObjects(List<Vector2> positions)
    {
        foreach (Vector2 pos in positions)
        {
            Vector3 spawnPos = new Vector3(pos.x, pos.y, 0);
            GameObject obj = Instantiate(fallingPrefab, spawnPos, Quaternion.identity);

            ApplyPhysics(obj, pos);

            if (Random.value > 0.4f)
                yield return new WaitForSeconds(Random.Range(0f, quickSpawnDelay));
        }
    }

    void ApplyPhysics(GameObject obj, Vector2 spawnPos)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        bool isLeftSide = spawnPos.x < centerX;
        bool isRightSide = spawnPos.x > centerX;

        if (useAngularVelocity)
        {
            float spin;

            if (isLeftSide)
            {
                spin = Random.Range(minAngularVelocity, -50f);
            }
            else if (isRightSide)
            {
                spin = Random.Range(50f, maxAngularVelocity);
            }
            else
            {
                spin = Random.Range(minAngularVelocity, maxAngularVelocity);
            }

            rb.angularVelocity = spin;
        }

        if (useHorizontalForce)
        {
            float forceX;

            if (isLeftSide)
            {
                forceX = Random.Range(0.5f, maxHorizontalForce);
            }
            else if (isRightSide)
            {
                forceX = Random.Range(minHorizontalForce, -0.5f);
            }
            else
            {
                forceX = Random.Range(minHorizontalForce, maxHorizontalForce);
            }

            rb.AddForce(new Vector2(forceX, 0f), ForceMode2D.Impulse);
        }
    }
}