using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//<3
public class ProcuderalGenerator : MonoBehaviour
{
    [Header("---Components")]
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private Transform _playerT;
    [SerializeField] private Player _player;
    [SerializeField] private RunnerGameManager _runnerGameManager;
    [Header("---Props")]
    [SerializeField] private float _heightMultiplier;
    [SerializeField] private float _randomOffsetRange = 100f;
    [SerializeField] private float _maxHeightDifference;

    [Tooltip("Maps parent")]
    [SerializeField] private Transform _map;
    [SerializeField] private MiniGameController _miniGameController;
    private float _randomSeed;
    private float _previousHeight;
    private int _nextSpawnX;
    private List<GameObject> _spawnedGrounds = new();




    void Start()
    {
        InitProcuderal();
    }
    [Button("Init Procuderal")]
    public void InitProcuderal()
    {
        GenerateGround();
    }
    private void GenerateGround()
    {
        _randomSeed = Random.Range(0, _randomOffsetRange);
        int groundCount = _runnerGameManager.CurrentLevel.Length;
        print(groundCount);
        float noiseInput = (_nextSpawnX * _runnerGameManager.CurrentFrequency) + _randomSeed;
        float height = Mathf.PerlinNoise(noiseInput, 0) * _heightMultiplier;

        _playerT.position = new Vector2(_nextSpawnX + 5, height + 15);

        for (int i = 0; i < groundCount; i++)
        {
            float heightDifference = Mathf.Abs(height - _previousHeight);
            if (heightDifference <= _maxHeightDifference)
            {
                height = _previousHeight;
            }
            Vector3 spawnPosition = new Vector3(_nextSpawnX, height, 0);
            GameObject tempGround = _poolManager.Get("Ground");
            Transform spawnedGrid = tempGround.transform;
            _spawnedGrounds.Add(tempGround);
            SpawnProp(height, new Vector3(_nextSpawnX, spawnPosition.y, 0));
            spawnedGrid.SetParent(_map);
            spawnedGrid.position = spawnPosition;
            _nextSpawnX += 1;
            _previousHeight = height;

            noiseInput = (_nextSpawnX * _runnerGameManager.CurrentFrequency) + _randomSeed;
            height = Mathf.PerlinNoise(noiseInput, 0) * _heightMultiplier;
        }
    }
    private void SpawnProp(float currentHeight, Vector3 blockPosition)
    {
        int randomMaterial = Random.Range(0, _runnerGameManager.CurrentMaterials.Count);
        Material selectedMaterial = _runnerGameManager.CurrentMaterials[randomMaterial];
        int possibility = Random.Range(0, 100);
        if (currentHeight <= selectedMaterial.HeightLine)
        {
            if (possibility <= selectedMaterial.Posibility)
            {
                GameObject tempCreatedObject = _poolManager.Get(selectedMaterial.Key);
                IMaterial spawnedMaterial = tempCreatedObject.GetComponent<IMaterial>();
                if (spawnedMaterial != null)
                    spawnedMaterial.Init(_player);
                tempCreatedObject.transform.position = blockPosition;
            }
        }


    }
    [Button("Re Generate")]
    public void ReGenerate()
    {
        _previousHeight = 0;
        _nextSpawnX = 0;
        int groundCount = _runnerGameManager.CurrentLevel.Length;
        print(groundCount);

        float noiseInput = (_nextSpawnX * _runnerGameManager.CurrentFrequency) + _randomSeed;
        float height = Mathf.PerlinNoise(noiseInput, 0) * _heightMultiplier;
        _playerT.position = new Vector2(_nextSpawnX + 5, height + 5);
        for (int i = 0; i < groundCount; i++)
        {
            float heightDifference = Mathf.Abs(height - _previousHeight);
            if (heightDifference <= _maxHeightDifference)
            {
                height = _previousHeight;
            }
            Vector3 spawnPosition = new Vector3(_nextSpawnX, height, 0);
            if (i >= _spawnedGrounds.Count)
            {
                Debug.LogError($"Hata: _spawnedGrounds listesi {i} indeksine erişmeye çalışıyor ama boyutu {_spawnedGrounds.Count}");
                return;
            }
            _spawnedGrounds[i].transform.position = spawnPosition;
            _nextSpawnX += 1;
            _previousHeight = height;
            noiseInput = (_nextSpawnX * _runnerGameManager.CurrentFrequency) + _randomSeed;
            height = Mathf.PerlinNoise(noiseInput, 0) * _heightMultiplier;
        }
    }
}
[System.Serializable]
public class ProcuderalPacket
{
    public float Frequency;
}