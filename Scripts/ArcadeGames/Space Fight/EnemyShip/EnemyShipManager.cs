using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipManager : SpaceFightEnvironment, IPoolObject
{
    [Header("-----Props")]
    [SerializeField] private string _enemyPoolKey;
    [SerializeField] private List<Vector3> _enemyShipsPoses = new();
    [SerializeField] private List<Vector3> Path = new();
    [SerializeField] private float _speed;
    [SerializeField] private float _spawnDelay;
    [SerializeField] public List<SFEnemyShip> _sFEnemyShips = new();

    public override string PoolKey { get => _enemyPoolKey; set => throw new NotImplementedException(); }

    public override event Action OnReturnAction;
    public override event Action OnGetAction;
    public override event Action OnCollect;

    public Player _player;
    public IEnumerator InitEnemyShipsIE(PoolManager poolManager)
    {
        for (int i = 0; i < _sFEnemyShips.Count; i++)
        {
            yield return new WaitForSeconds(_spawnDelay);
            _sFEnemyShips[i].Init(_player, poolManager, Path, _enemyShipsPoses[i], _speed);
        }
    }
    private int _deadShipCount;
    public void CheckAndKillShip(SFEnemyShip sFEnemyShip)
    {
        if (sFEnemyShip == null) return;
        if (sFEnemyShip.IsDead) return;
        _deadShipCount++;
        if (_sFEnemyShips.Contains(sFEnemyShip))
        {
            if (_deadShipCount >= _sFEnemyShips.Count)
            {
                Debug.Log("All ships are dead");
                CollectMe(this);
                return;
            }
        }
    }
    private int _attackReadyCount = 0;
    private bool _canStartShake = true;
    public void CheckAndStartAttack()
    {
        _attackReadyCount++;
        if (_attackReadyCount < _sFEnemyShips.Count)
        {
            return;
        }
        if (_canStartShake && _attackReadyCount >= _sFEnemyShips.Count)
        {
            _sFEnemyShips.ForEach((ship) => { ship.StartShake(); });
            _canStartShake = false;
        }
        int randomEnemyShip = UnityEngine.Random.Range(0, _sFEnemyShips.Count);
        if (_sFEnemyShips[randomEnemyShip] == null) return;
        _sFEnemyShips[randomEnemyShip].Attack();
        print("sa");
        StartCoroutine(AttackDelay());
    }
    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_spawnDelay);
        CheckAndStartAttack();
    }
    public override void CollectMe(MonoBehaviour collectable)
    {
        _poolManager.Return(gameObject);
    }

    public override void InitSpaceFightEnvironment(PoolManager poolManager, Transform parent = null, Player player = null)
    {
        _poolManager = poolManager;
        _player = player;
        StartCoroutine(InitEnemyShipsIE(poolManager));
    }

    public override void OnGet()
    {
    }

    public override void OnInit(PoolManager poolManager)
    {
    }

    public override void OnReturn()
    {
        _attackReadyCount = 0;
    }
}