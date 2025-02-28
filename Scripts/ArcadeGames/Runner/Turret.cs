using UnityEngine;

public class Turret : Collectables, IPoolObject, IMaterial
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _delay;

    private Player _player;
    public string PoolKey { get => "Turret"; set => value = default; }

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
        Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.Player = _player;
    }
    public override void CollectMe()
    {
        _player.Die();
    }
}
