using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyType", menuName = "ScriptableObjects/CrossRoad/EnemyType", order = 1)]
public class CrossRoad_EnemyType : ScriptableObject
{
    public float Speed;
    public int Health;
    public float ScoreAmount;
    public Sprite EnemySprite;
    public ActionSO OnStart_EnemyEvent, OnDead_EnemyEvent, OnHit_EnemyEvent, OnInterval_EnemyEvent;
    public float IntervalEvent_TimeSpan;
}
[Serializable]
public abstract class EnemyEventTypeSO : ScriptableObject
{
    public abstract void ExecuteAction(GameObject @object);
}