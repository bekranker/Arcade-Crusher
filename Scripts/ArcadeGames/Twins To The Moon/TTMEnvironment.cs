using UnityEngine;

public abstract class TTMEnvironment : Collectables
{
    protected LoseScreen _loseScreen;
    protected TwinsToTheMoonHandler _twinsToTheMoonHandler;
    protected PoolManager _poolManager;


    public void Initialize(TwinsToTheMoonHandler twinsToTheMoonHandler, LoseScreen loseScreen, PoolManager poolManager)
    {
        _poolManager = poolManager;
        _loseScreen = loseScreen;
        _twinsToTheMoonHandler = twinsToTheMoonHandler;
    }
    public abstract void OnDie();
}