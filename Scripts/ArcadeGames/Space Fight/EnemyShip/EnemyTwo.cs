using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
public class EnemyTwo : SFEnemyShip
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
        Die();
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
    private int _direction = -1;
    public override void Move()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = _path[_pathIndex];

        // Ortaya yukarıdan bir eğri kat
        Vector3 midPoint = (startPos + endPos) / 2 + Vector3.up * _direction * 2f;

        Vector3[] curvePath = new Vector3[] { startPos, midPoint, endPos };

        transform.DOPath(curvePath, _movementSpeed, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _direction *= -1;
                UpdatePathIndex();
                Move();
            });

    }
    private bool _goingForward = true;
    private void UpdatePathIndex()
    {
        if (_goingForward)
        {
            _pathIndex++;
            if (_pathIndex >= _path.Count)
            {
                _pathIndex = _path.Count - 2; // sondan bir önceki indeks
                _goingForward = false;
            }
        }
        else
        {
            _pathIndex--;
            if (_pathIndex < 0)
            {
                _pathIndex = 1; // baştan bir sonraki indeks
                _goingForward = true;
            }
        }
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

    public override void Attack()
    {
    }

    public override void StartShake()
    {
    }
    #endregion
}