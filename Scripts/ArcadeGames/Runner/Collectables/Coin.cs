using System;
using UnityEngine;
public class Coin : Collectables, IMaterial
{
    public override string PoolKey { get => "Coin"; set => value = default; }
    GeneralScoreHandler _generalScoreHandler;
    Player _player;
    PoolManager _poolManager;
    public override event Action OnCollect;


    public override void CollectMe(MonoBehaviour mono)
    {
        _generalScoreHandler.IncreaseScore(100);
        OnCollect?.Invoke();
        _poolManager.Return(gameObject);
    }

    public void Init(Player player)
    {
        _player = player;
        _poolManager = FindAnyObjectByType<PoolManager>();
    }

    public override void OnInit()
    {
        _generalScoreHandler = FindAnyObjectByType<GeneralScoreHandler>();
    }

    public override void OnReturn()
    {
    }

    public override void OnGet()
    {
    }
}
