using System.Collections.Generic;
using ArcadeGames.CrossRoad;
using ArcadeGames.Runner;
using UnityEngine;
using UnityEngine.UI;
namespace ArcadeCrusher.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("-----Props")]
        [SerializeField] private int _maxBulletCount;

        [Header("-----Components")]
        [SerializeField] private BulletParent<CrossRoad_Enemy> _bulletPrefab;

        [Header("-----UI & Canvas")]
        [SerializeField] private List<Image> _bulletCountImages;

        private Player_Actions _inputActions;
        private Vector2 previousInput = Vector2.zero;
        private int _bulletCounter;

        void Awake()
        {
            _inputActions = new();
            _bulletCounter = _maxBulletCount;
        }

        void OnEnable()
        {
            _inputActions.Enable();
        }
        void OnDisable()
        {
            _inputActions.Disable();
        }

        void Update()
        {
            Vector2 currentInput = _inputActions.Player.ShootZX.ReadValue<Vector2>();
            // One-shot detection: went from no input to some input
            if (previousInput == Vector2.zero && currentInput != Vector2.zero)
            {
                Shoot(currentInput);
            }
            previousInput = currentInput;
        }

        void Shoot(Vector2 direction)
        {
            if (_bulletCounter <= 0) return;

            BulletParent<CrossRoad_Enemy> spawnedBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            spawnedBullet.DirectionToGo.x = direction.x;
            DecreaseBulletCount();
        }
        public void DecreaseBulletCount()
        {
            if (_bulletCounter <= 0)
            {
                Debug.LogWarning("No bullets left to decrease.");
                return;
            }
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