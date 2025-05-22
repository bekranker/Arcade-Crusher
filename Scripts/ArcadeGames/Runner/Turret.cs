using System;
using UnityEngine;

public class Turret : Collectables, IMaterial
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _delay;

    private Player _player;

    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;

    private PoolManager _poolManager;
    public override string PoolKey { get => "Turret"; set => value = default; }

    void Start()
    {
        InvokeRepeating("LaunchProjectile", _delay, _delay);
    }
    public void Init(Player player)
    {
        _player = player;
    }
    public void LaunchProjectile()
    {
        Bullet bullet = _poolManager.Get("bullet").GetComponent<Bullet>();
        bullet.Player = _player;
    }
    public override void CollectMe(MonoBehaviour mono)
    {
        _player.Die();
        OnCollect?.Invoke();
    }

    public override void OnInit(PoolManager poolManager)
    {
        _poolManager = poolManager;
    }

    public override void OnReturn()
    {
    }

    public override void OnGet()
    {
    }
}
