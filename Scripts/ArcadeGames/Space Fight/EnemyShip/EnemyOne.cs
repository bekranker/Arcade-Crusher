using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
public class EnemyOne : SFEnemyShip
{
    [Header("---Components---")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _shootPoint;
    [Header("---Props---")]
    [SerializeField] float _maximumHealth;
    [SerializeField] float _movementSpeed;
    [SerializeField] float _damageCount;
    [SerializeField] float _shootDelay;
    [SerializeField] private EnemyShipManager _enemyShipManager;
    private float _healthCounter;


    public override string PoolKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool Attacking;

    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;

    private List<Vector3> _path = new();
    private Vector3 _targetSeat;
    private PoolManager _poolManager;

    public override void ApplyDamage(float amount)
    {
        if (IsDead || amount <= 0) return;
        DOTween.Kill(_spriteRenderer.transform);
        _spriteRenderer.transform.DOPunchScale(Vector3.one * 0.3f, .15f);
        _healthCounter -= amount;
        if (_healthCounter <= 0)
        {
            Die();
            return;
        }
    }

    public override void CollectMe(MonoBehaviour collectable)
    {
        _enemyShipManager._player.TakeDamage(1);
    }
    public override bool IsDead { get; set; }
    public override void Die()
    {
        _spriteRenderer.transform.DOKill();
        _enemyShipManager.CheckAndKillShip(this);
        IsDead = true;
    }

    public override void Init(Player player, PoolManager poolManager, List<Vector3> poses, Vector3 position = default, float speed = 0)
    {
        _movementSpeed = speed;
        _healthCounter = _maximumHealth;
        _spriteRenderer.enabled = true;
        _spriteRenderer.transform.localScale = Vector3.one;
        _spriteRenderer.transform.DOKill();
        _targetSeat = position;
        _poolManager = poolManager;
        _path = poses;
        Move();
    }
    int _pathIndex;
    public override void Move()
    {
        if (_path.Count == 0 || _path == null)
        {
            CallStartAttack();
            return;
        }
        Vector3 startPos = transform.position;
        Vector3 endPos = _path[_pathIndex];

        // Ortaya yukarıdan bir eğri kat
        Vector3 midPoint = (startPos + endPos) / 2 + Vector3.up * 2f;

        Vector3[] curvePath = new Vector3[] { startPos, midPoint, endPos };

        transform.DOPath(curvePath, _movementSpeed, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                UpdatePathIndex();
            });

    }
    private void UpdatePathIndex()
    {
        _pathIndex++;
        if (_pathIndex >= _path.Count)
        {
            CallStartAttack();
            return;
        }
        Move();
    }
    private void CallStartAttack()
    {
        transform.DOLocalMove(_targetSeat, _movementSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                _enemyShipManager.CheckAndStartAttack();
            });
    }
    public override void StartShake()
    {
        transform.DOLocalMoveX(transform.position.x - 1.5f, _movementSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOLocalMoveX(_targetSeat.x, _movementSpeed).SetEase(Ease.Linear);
        }).SetLoops(-1, LoopType.Yoyo);
    }
    public override void Attack()
    {
        ShootAbove();
    }
    private void ShootAbove()
    {
        //start Shooting or other actions
        if (_healthCounter <= 0) return;
        Bullet bullet = _poolManager.Get("Bullet").GetComponent<Bullet>();
        bullet._speed = 10;
        bullet.DirectionToGo = Vector3.down;
        bullet.InitBullet(_shootPoint, _poolManager);
    }
    #region Unused
    public override void OnGet()
    {
    }

    public override void OnInit(PoolManager poolManager)
    {
    }

    public override void OnReturn()
    {
    }
    #endregion
}