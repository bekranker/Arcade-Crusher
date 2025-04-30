using UnityEngine;
using System.Collections.Generic;
using ArcadeGames.CrossRoad;
using ArcadeCrusher.Player;
using TMPro;

namespace ArcadeGames
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private CrossRoad_Enemy objectToSpawn;
        [SerializeField] private List<Transform> spawnPoints; // List of spawn points
        [SerializeField] private List<CrossRoad_WaveType> _waves = new();
        [SerializeField] private PlayerShoot _playerShoot;
        [Header("---UI & Canvas---")]
        [SerializeField] private TMP_Text _counterTMP;
        private float _timer, _waveTimer;
        private int _waveSystemCount;

        private void Update()
        {
            _timer += Time.deltaTime;
            _waveTimer += Time.deltaTime;
            if (_waveTimer >= _waves[_waveSystemCount].TotalTime)
            {
                if (_waveSystemCount >= _waves.Count)
                {
                    Debug.LogError("Wave system count is out of range.");
                    return;
                }
                _waveSystemCount++;
                _waveTimer = 0f;
            }
            if (_timer >= _waves[_waveSystemCount].SpawnInterval)
            {
                SpawnObject();
                _timer = 0f;
            }
            if (_counterTMP != null)
            {
                _counterTMP.text = $"{_waves[_waveSystemCount].TotalTime - _waveTimer:F2}";
            }
        }

        private void SpawnObject()
        {
            if (objectToSpawn != null && spawnPoints != null && spawnPoints.Count > 0)
            {
                // Select a random spawn point from the list
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                CrossRoad_Enemy spawnedEnemy = Instantiate(objectToSpawn, randomSpawnPoint.position, randomSpawnPoint.rotation);
                spawnedEnemy.Init(_waves[_waveSystemCount], _playerShoot);
                spawnedEnemy.DirectionToGo = randomSpawnPoint.position * -1;
            }
            else
            {
                Debug.LogWarning("Spawner is missing required references or spawn points.");
            }
        }
    }
}