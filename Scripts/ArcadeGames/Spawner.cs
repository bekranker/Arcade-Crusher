using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private List<Transform> spawnPoints; // List of spawn points
    [SerializeField] private float spawnInterval = 2f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            SpawnObject();
            _timer = 0f;
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn != null && spawnPoints != null && spawnPoints.Count > 0)
        {
            // Select a random spawn point from the list
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Instantiate(objectToSpawn, randomSpawnPoint.position, randomSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Spawner is missing required references or spawn points.");
        }
    }
}