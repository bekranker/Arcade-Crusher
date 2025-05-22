using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace ArcadeCrusher.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("-----Props")]
        [SerializeField] private int _maxBulletCount;

        [Header("-----Components")]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private Transform _bulletSpawnPoint;

        [Header("-----UI & Canvas")]
        [SerializeField] private List<Image> _bulletCountImages;

        private Player_Actions _inputActions;
        private Vector2 previousInput = Vector2.zero;
        public event Action OnShoot;
        private int _bulletCounter;

        void Awake()
        {
            _inputActions = new();
            _bulletCounter = _maxBulletCount;
        }

        void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Player.InteractValue.performed += Shoot;
        }
        void OnDisable()
        {
            _inputActions.Player.InteractValue.performed -= Shoot;
            _inputActions.Disable();
        }

        void Shoot(InputAction.CallbackContext context)
        {
            if (_bulletCounter <= 0) return;
            OnShoot?.Invoke();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = transform.up;
            Bullet spawnedBullet = _poolManager.Get("Bullet").GetComponent<Bullet>();
            spawnedBullet._speed = 30;
            spawnedBullet.DirectionToGo = direction;
            spawnedBullet.InitBullet(_bulletSpawnPoint, _poolManager);
            DecreaseBulletCount();
        }
        public void DecreaseBulletCount()
        {
            _bulletCounter--;
            UpdateBulletCountUI();
        }
        public void IncreaseBulletCount(int count)
        {
            if (_bulletCounter + _maxBulletCount > _maxBulletCount)
            {
                _bulletCounter = _maxBulletCount;
                UpdateBulletCountUI();
                return;
            }
            _bulletCounter += count;
            UpdateBulletCountUI();
        }
        private void UpdateBulletCountUI()
        {
            for (int i = 0; i < _bulletCountImages.Count; i++)
            {
                _bulletCountImages[i].enabled = i < _bulletCounter;
            }
        }
    }
}