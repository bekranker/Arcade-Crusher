using System;
using UnityEngine;

public class Lazer : SpaceFightEnvironment
{
    [SerializeField] private float lifetime = 5f;

    public override string PoolKey { get => "ShipLazer"; set => throw new NotImplementedException(); }

    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;
    private Player _player;
    public override void CollectMe(MonoBehaviour collectable)
    {
        _player.TakeDamage(1);
    }

    public override void InitSpaceFightEnvironment(PoolManager poolManager, Transform parent = null, Player player = null)
    {
        _player = player;
        _poolManager = poolManager;
    }
    private void DeActivateLazer()
    {
        _poolManager.Return(gameObject);
    }
    public override void OnGet()
    {
    }

    public override void OnInit(PoolManager poolManager)
    {
    }

    public override void OnReturn()
    {
    }
}