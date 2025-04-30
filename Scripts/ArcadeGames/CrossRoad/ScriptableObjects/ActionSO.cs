using UnityEngine;

public class ActionSO : ScriptableObject
{
    [Header("-----Props")]
    [SerializeField] EnemyEventTypeSO _enemyEventType;
    public virtual void Execute(GameObject target)
    {
        _enemyEventType.ExecuteAction(target);
    }
}
