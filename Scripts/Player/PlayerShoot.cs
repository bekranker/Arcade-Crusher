using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcadeCrusher.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("-----Props")]
        [SerializeField] private int _bulletCount;

        [Header("-----Components")]
        [SerializeField] private Bullet _bulletPrefab;

        private Player_Actions _inputActions;
        private Vector2 previousInput = Vector2.zero;
        private int _bulletCounter;

        void Awake()
        {
            _inputActions = new();
            _bulletCounter = _bulletCount;
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

            Bullet spawnedBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            spawnedBullet.DirectionToGo.x = direction.x;
            _bulletCounter--;
        }
    }
}