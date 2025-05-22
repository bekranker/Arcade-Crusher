using System;
using UnityEngine;

public abstract class SpaceFightEnvironment : Collectables
{
    protected PoolManager _poolManager;
    public abstract void InitSpaceFightEnvironment(PoolManager poolManager, Transform parent = null, Player player = null);
}