using System.Collections;
using UnityEngine;
using System;
public class Bullet : Collectables
{
    [Header("-----Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] public float _speed;
    public Vector2 DirectionToGo = Vector2.up;

    [Header("-----Damage Props")]
    [SerializeField] private float _damageAmount;
    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;

    public Player Player { get; set; }
    public override string PoolKey { get => "Bullet"; set => value = "Bullet"; }
    private PoolManager _poolManager;

    IEnumerator Start()
    {
        transform.right = DirectionToGo;
        yield return new WaitForSeconds(3);
        _poolManager.Return(gameObject);
    }
    void Update()
    {
        BulletMove();
    }
    public void InitBullet(Transform bulletSpawnPoint, PoolManager poolManager)
    {
        transform.position = bulletSpawnPoint.position;
        _rb.linearVelocity = Vector2.zero;
        _poolManager = poolManager;
    }
    public void BulletMove()
    {
        _rb.linearVelocity = transform.right * _speed;
    }
    public override void CollectMe(MonoBehaviour mono)
    {
        Player.Die();
        OnCollect?.Invoke();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.TryGetComponent(out IDamageProp damageProp))
        {
            damageProp.ApplyDamage(_damageAmount);
            _poolManager.Return(gameObject);
        }
    }
    public override void OnInit(PoolManager poolManager)
    {
        _poolManager = poolManager;
        transform.right = DirectionToGo;
    }

    public override void OnReturn()
    {
    }

    public override void OnGet()
    {
    }
}