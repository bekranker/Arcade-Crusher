using System.Collections;
using UnityEngine;
using DG.Tweening;
using ArcadeGames.CrossRoad;
using System;
public class Bullet : Collectables, IPoolObject
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    public Vector2 DirectionToGo = Vector2.up;

    public override event Action OnCollect;

    public Player Player { get; set; }
    public string PoolKey { get => "Bullet"; set => value = default; }


    IEnumerator Start()
    {
        transform.right = DirectionToGo;
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
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
}