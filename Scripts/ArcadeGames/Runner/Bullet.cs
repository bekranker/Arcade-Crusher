using System.Collections;
using UnityEngine;

public class Bullet : Collectables, IPoolObject, ICollectable
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    public Vector2 DirectionToGo;


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
    public override void CollectMe()
    {
        Player.Die();
    }
}