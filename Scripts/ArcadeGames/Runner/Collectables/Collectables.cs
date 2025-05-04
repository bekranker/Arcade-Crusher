using System;
using UnityEngine;

public abstract class Collectables : MonoBehaviour, ICollectable<MonoBehaviour>, IPoolObject
{
    public abstract string PoolKey { get; set; }

    public abstract void CollectMe(MonoBehaviour collectable);

    public abstract void OnInit();

    public abstract void OnReturn();

    public abstract void OnGet();

    public abstract event Action OnCollect;
}