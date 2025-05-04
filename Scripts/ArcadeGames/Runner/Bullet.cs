using System.Collections;
using UnityEngine;
using DG.Tweening;
using ArcadeGames.CrossRoad;
using System;
public class Bullet : Collectables
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    public Vector2 DirectionToGo = Vector2.up;

    public override event Action OnCollect;

    public Player Player { get; set; }
    public override string PoolKey { get => "Bullet"; set => value = default; }
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
    public void BulletMove()
    {
        _rb.linearVelocity = transform.right * _speed;
    }
    public override void CollectMe(MonoBehaviour mono)
    {
        Player.Die();
        OnCollect?.Invoke();
    }

    public override void OnInit()
    {
        transform.right = DirectionToGo;
    }

    public override void OnReturn()
    {
    }

    public override void OnGet()
    {
    }
}