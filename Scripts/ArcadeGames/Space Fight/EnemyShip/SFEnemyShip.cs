using System.Collections.Generic;
using UnityEngine;

public abstract class SFEnemyShip : Collectables, IDamageProp
{
    public abstract void StartShake();
    public abstract bool IsDead { get; set; }
    public abstract void Init(Player player, PoolManager poolManager, List<Vector3> poses, Vector3 position = default, float speed = 0);
    public abstract void ApplyDamage(float amount);
    public abstract void Die();
    /// <summary>
    /// Abstract method for movement, to be implemented by derived classes
    /// </summary>
    public abstract void Move();
    public abstract void Attack();
}