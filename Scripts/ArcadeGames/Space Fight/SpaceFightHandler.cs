using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// --------------------------- May 12, 2025 ---------------------------
/// 
/// make spawner that spawn props with time interval
/// make enemies
///     Type 1
///     Type 2
///     Type 3
///     Type 4
///     Type 5
/// make Asteroids
/// make Black Holes
/// 
/// --------------------------- May 19, 2025 ---------------------------
/// 
/// make polish
///     shoot
///     damage
///     shakes
/// Add Sprites
///     Player
///     Enemy Ships
///     Asteroids
///     Black Hole
///     
/// --------------------------- May 26, 2025 ---------------------------




public class SpaceFightHandler : MonoBehaviour
{
    [Header("-----UI")]
    [SerializeField] private Slider _remainingTime;
    [SerializeField] private float _maxTime;

    private float _remainingTimeValue = 0;

    [Header("-----Spanwer")]
    [SerializeField] private Transform _propParent;
    [SerializeField] private List<HighGroundSCB> _highGroundSCBs;
    [SerializeField] private float _spawnInterval = 1f;
    private int _highGroundIndex = 0;
    private float _targetScore;
    private float _counter;

    [Header("-----Components")]
    [SerializeField] private Player _player;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private GeneralHearthManager _generalHearthManager;
    [SerializeField] private GeneralScoreHandler _generalScoreHandler;
    void Awake()
    {
        _remainingTime.maxValue = _maxTime;
        _remainingTimeValue = _maxTime;
        _remainingTime.value = _remainingTimeValue;
        _counter = _spawnInterval;
    }
    private void DecreaseSlider()
    {
        if (_remainingTimeValue > 0)
        {
            _remainingTimeValue -= Time.deltaTime;
            _remainingTime.value = _remainingTimeValue;
        }
        else
        {
            _remainingTimeValue = _maxTime;
            _generalHearthManager.DecreaseHealth();
        }
    }
    private void Update()
    {
        SetTargtetScore();
        DecreaseSlider();
        SpawnInterval();
    }
    private void SpawnInterval()
    {
        if (_counter > 0)
        {
            _counter -= Time.deltaTime;
        }
        else
        {
            Spawner();
            _counter = _spawnInterval;
        }
    }
    private int _spawnCounter;
    private void Spawner()
    {
        int randomSpawnProp = Random.Range(0, _highGroundSCBs[_highGroundIndex].Environments.Count);
        TTMEnvironmentLogic selectedEnvironmentLogic = _highGroundSCBs[_highGroundIndex].Environments[randomSpawnProp];
        print(selectedEnvironmentLogic);
        int _maximumSpawnCount = selectedEnvironmentLogic.MaxSpawnCount;
        if (_spawnCounter >= _maximumSpawnCount) return;
        foreach (string propName in selectedEnvironmentLogic.Environments)
        {
            int possibilty = Random.Range(0, 100);
            if (possibilty <= selectedEnvironmentLogic.Possibility)
            {
                SpaceFightEnvironment pooledObject = _poolManager.Get(propName).GetComponent<SpaceFightEnvironment>();
                pooledObject.InitSpaceFightEnvironment(_poolManager, null, _player);
                print(propName);
                _spawnCounter++;
            }
        }
    }
    private void SetTargtetScore()
    {
        _targetScore = _highGroundSCBs[_highGroundIndex].MaxHeight;
        if (_generalScoreHandler.ScoreCounter >= _targetScore)
        {
            _highGroundIndex++;
            if (_highGroundIndex >= _highGroundSCBs.Count)
            {
                _highGroundIndex = 0;
            }
        }
    }
}