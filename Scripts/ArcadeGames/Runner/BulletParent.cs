using System;
using System.Collections;
using ArcadeGames.CrossRoad;
using UnityEngine;

namespace ArcadeGames.Runner
{
    public abstract class BulletParent<T> : MonoBehaviour, ICollectable<T>, IPoolObject
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed;
        public Vector2 DirectionToGo = Vector2.up;

        public event Action OnReturnAction;
        public event Action OnGetAction;

        public Player Player { get; set; }
        public string PoolKey { get => "Bullet"; set => value = default; }
        private PoolManager _poolManager;

        IEnumerator DisableMe()
        {
            yield return new WaitForSeconds(3);
        }
        public abstract void OnBulletInit();

        void Update()
        {
            BulletMove();
        }
        public void BulletMove()
        {
            _rb.linearVelocity = transform.right * _speed;
        }
        public abstract void CollectMe(T collectable);

        public void OnReturn()
        {
        }

        public void OnGet()
        {
            transform.right = DirectionToGo;
        }

        public void OnInit(PoolManager poolManager)
        {
            _poolManager = poolManager;
        }
    }
}