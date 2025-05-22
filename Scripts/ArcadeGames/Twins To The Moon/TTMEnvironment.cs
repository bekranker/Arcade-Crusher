using UnityEngine;

public abstract class TTMEnvironment : Collectables
{
    protected TwinsToTheMoonHandler _twinsToTheMoonHandler;
    protected PoolManager _poolManager;


    public void Initialize(TwinsToTheMoonHandler twinsToTheMoonHandler, LoseScreen loseScreen, PoolManager poolManager)
    {
        _poolManager = poolManager;
        _twinsToTheMoonHandler = twinsToTheMoonHandler;
    }
    public abstract void OnDie();
}