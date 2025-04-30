using System.Collections;
using ArcadeGames.CrossRoad;
using UnityEngine;

namespace ArcadeGames.Runner
{
    public abstract class BulletParent<T> : MonoBehaviour, ICollectable<T>
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed;
        public Vector2 DirectionToGo = Vector2.up;


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
        public abstract void CollectMe(T collectable);
    }
}