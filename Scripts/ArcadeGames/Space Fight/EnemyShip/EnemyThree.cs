using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
public class EnemyThree : SFEnemyShip
{
    [Header("---Components---")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _shootPoint;
    [Header("---Props---")]
    [SerializeField] private Vector3 _directionOfBullet;
    [SerializeField] float _maximumHealth;
    [SerializeField] float _movementSpeed;
    [SerializeField] float _damageCount;
    [SerializeField] float _shootDelay;
    [SerializeField] private EnemyShipManager _enemyShipManager;
    private float _healthCounter;


    public override string PoolKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public override bool IsDead { get; set; }

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
    public bool Attacking;
    public override void Move()
    {
        Vector3 targetPos = _path[_pathIndex];
        transform.DOMove(targetPos, _movementSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            _enemyShipManager.CheckAndStartAttack();
        });
    }
    public override void Attack()
    {
        if (Attacking)
        {
            _enemyShipManager.CheckAndStartAttack();
            return;
        }
        Attacking = true;
        transform.DOLocalMove(_targetSeat, _movementSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                //start Shooting or other actions
                ShootLazer();
            });
    }

    private void ShootLazer()
    {
        print("Shooting");
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

    public override void StartShake()
    {
    }
    #endregion
}