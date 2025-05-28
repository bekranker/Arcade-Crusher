using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemyFour : SFEnemyShip
{
    [SerializeField] private EnemyShipManager _enemyShipManager;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _stopDistance = 0.1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;
    public override bool IsDead { get; set; }
    public override string PoolKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;

    private Player _player;
    private PoolManager _poolManager;

    public bool Attacking;
    private bool _canFollow;
    private float _healthCounter;
    private Vector3 _target;

    void Update()
    {
        if (_player != null)
            _target = _player.transform.position;
        Move();
    }
    public override void ApplyDamage(float amount)
    {
    }

    public override void Attack()
    {
        if (Attacking) return;
        Attacking = true;
        _canFollow = true;
    }

    public override void CollectMe(MonoBehaviour collectable)
    {
    }

    public override void Die()
    {
    }

    public override void Init(Player player, PoolManager poolManager, List<Vector3> poses, Vector3 position = default, float speed = 0)
    {
        _player = player;
        _poolManager = poolManager;
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.position = new Vector3(Random.Range(-9, 9), Random.Range(-9, 9), 0);
        _collider.enabled = false;
        _spriteRenderer.DOFade(1, 1.5f).OnComplete(() =>
        {
            _collider.enabled = true;
            Attack();
        });
    }

    public override void Move()
    {
        Vector3 direction = (_target - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _target);
        transform.right = direction;
        if (distance < _stopDistance) return;
        if (!Attacking) return;
        transform.position += direction * Time.deltaTime * _movementSpeed;
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

    public override void StartShake()
    {
    }

}