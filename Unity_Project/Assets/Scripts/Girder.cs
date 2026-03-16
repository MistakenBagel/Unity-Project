using UnityEngine;
using System.Collections;

public class Girder : MonoBehaviour
{
    public GameObject girderPrefab;

    public float minX = 2f;
    public float maxX = 14f;
    public float spawnY = 1f;

    public int girdersToSpawn = 3;
    public float delayBetweenGirders = 0.4f;

    public float girderLifetime = 3f;

    public void SpawnWave() // Called by FightController
    {
        StartCoroutine(SpawnGirders());
    }

    IEnumerator SpawnGirders()
    {
        for (int i = 0; i < girdersToSpawn; i++)
        {
            int randomStep = Random.Range(Mathf.RoundToInt(minX), Mathf.RoundToInt(maxX));
            float randomX = randomStep + 0.5f;

            Vector2 spawnPosition = new Vector2(randomX, spawnY);

            GameObject girder = Instantiate(girderPrefab, spawnPosition, Quaternion.identity);

            Destroy(girder, girderLifetime);

            yield return new WaitForSeconds(delayBetweenGirders);
        }
    }
}