using UnityEngine;

public class Girder : MonoBehaviour
{
    public GameObject girderPrefab;

    public float minX = 2f;
    public float maxX = 14f;
    public float spawnY = 1f;

    public float spawnInterval = 7f;
    public float girderLifetime = 3f;

    void Start()
    {
        InvokeRepeating("SpawnGirder", 0f, spawnInterval);
    }

    void SpawnGirder()
    {
        float randomX = Random.Range(minX, maxX);

        Vector2 spawnPosition = new Vector2(randomX, spawnY);

        GameObject girder = Instantiate(girderPrefab, spawnPosition, Quaternion.identity);

        Destroy(girder, girderLifetime);
    }
}