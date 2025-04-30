using System;
using UnityEngine;
public class Coin : Collectables, IPoolObject, IMaterial
{
    public string PoolKey { get => "Coin"; set => value = default; }
    GeneralScoreHandler _generalScoreHandler;
    Player _player;

    public override event Action OnCollect;

    void Start()
    {
        _generalScoreHandler = FindAnyObjectByType<GeneralScoreHandler>();
    }
    public override void CollectMe(MonoBehaviour mono)
    {
        _generalScoreHandler.IncreaseScore(100);
        OnCollect?.Invoke();
        Destroy(gameObject);
    }

    public void Init(Player player)
    {
        _player = player;
    }
}
