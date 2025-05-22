using System;
using UnityEngine;
public class Coin : Collectables, IMaterial
{
    public override string PoolKey { get => "Coin"; set => value = default; }
    GeneralScoreHandler _generalScoreHandler;
    Player _player;
    PoolManager _poolManager;
    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;

    public override void CollectMe(MonoBehaviour mono)
    {
        _generalScoreHandler.IncreaseScore(100);
        OnCollect?.Invoke();
        _poolManager.Return(gameObject);
    }

    public void Init(Player player)
    {
        _player = player;
    }

    public override void OnInit(PoolManager poolManager)
    {
        _generalScoreHandler = FindAnyObjectByType<GeneralScoreHandler>();
        _poolManager = poolManager;
    }

    public override void OnReturn()
    {
    }

    public override void OnGet()
    {
    }
}
