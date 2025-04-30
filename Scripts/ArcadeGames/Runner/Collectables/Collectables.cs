using System;
using UnityEngine;

public abstract class Collectables : MonoBehaviour, ICollectable<MonoBehaviour>
{
    public abstract void CollectMe(MonoBehaviour collectable);
    public abstract event Action OnCollect;
}