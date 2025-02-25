using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

//<3
public class ProcuderalGenerator : MonoBehaviour
{
    [Header("---Components")]
    [SerializeField] private PoolManager _poolManager;
    [SerializeReference] private SerializedDictionary<int, ProcuderalRunnerLevelSCB> _levels = new();
    [Header("---Props")]
    [SerializeField] private float _heightMultiplier;
    [SerializeField] private float _randomOffsetRange = 100f;
    [SerializeField] private float _maxHeightDifference;

    [Tooltip("Maps parent")]
    [SerializeField] private Transform _map;
    [SerializeField] private MiniGameController _miniGameController;
    public int LevelIndex, BiomIndex;
    private float _randomSeed;
    private float _previousHeight;
    private int _nextSpawnX;
    private List<GameObject> _spawnedGrounds = new();
    private bool _startTheGame;

    void Update()
    {
        if (_startTheGame) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startTheGame = true;
            InitProcuderal();
        }
    }


    [Button("Init Procuderal")]
    public void InitProcuderal()
    {
        _miniGameController.ContunieToPlay();
        GenerateGround();
    }
    private void GenerateGround()
    {
        int groundCount = _levels[LevelIndex].Length;
        for (int i = 0; i <= groundCount; i++)
        {
            float noiseInput = (_nextSpawnX * _levels[LevelIndex].Biom[BiomIndex].PrePucket.Frequency) + _randomSeed;
            float height = Mathf.PerlinNoise(noiseInput, 0) * _heightMultiplier;

            float heightDifference = Mathf.Abs(height - _previousHeight);
            if (heightDifference <= _maxHeightDifference)
            {
                height = _previousHeight;
            }
            Vector3 spawnPosition = new Vector3(_nextSpawnX, height, 0);
            GameObject tempGround = _poolManager.Get("Ground");
            Transform spawnedGrid = tempGround.transform;
            _spawnedGrounds.Add(tempGround);
            spawnedGrid.SetParent(_map);
            spawnedGrid.localPosition = spawnPosition;
            _nextSpawnX += 1;
            _previousHeight = height;
        }
    }
    [Button("Re Generate")]
    public void ReGenerate()
    {
        foreach (GameObject ground in _spawnedGrounds)
        {
            ground.SetActive(false);
        }
        _spawnedGrounds?.Clear();
        GenerateGround();

    }
}


[System.Serializable]
public class ProcuderalPacket
{
    public float Frequency;
}