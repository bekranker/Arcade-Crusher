using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveType", menuName = "ScriptableObjects/CrossRoad/WaveType", order = 1)]
public class CrossRoad_WaveType : ScriptableObject
{
    public List<CrossRoad_EnemyType> CrossRoad_EnemyTypes = new();
    public float SpawnInterval;
    public float TotalTime;

    public CrossRoad_EnemyType GetRandomEnemyType() => CrossRoad_EnemyTypes[Random.Range(0, CrossRoad_EnemyTypes.Count)];
}