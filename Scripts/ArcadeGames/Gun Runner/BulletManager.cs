using UnityEngine;
using UnityEngine.InputSystem;

public class BulletManager : MonoBehaviour
{
    [Header("---Bullet Props")]
    public int BulletCount;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Bullet BulletPrefab;
    public Vector2 Direction;
    public Vector2 MovementInput;
    private Player_Actions _playerAction;
    void Awake()
    {
        _playerAction = new();
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (BulletCount - 1 < 0) return;
        if (MovementInput.x == 0 && MovementInput.y == 0) return;
        Bullet spawnedBullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        if (Direction.y <= 0)
        {
            spawnedBullet.DirectionToGo = new Vector2(-Direction.x, 0);
        }
        else
        {
            spawnedBullet.DirectionToGo = new Vector2(-Direction.y, 0);
        }
    }
    void OnEnable()
    {
        _playerAction.Enable();
        _playerAction.Player.Look.performed += CalculateDirection;
        _playerAction.Player.Look.canceled += CalculateDirection;
        _playerAction.Player.Attack.performed += Shoot;
        _playerAction.Player.Attack.canceled += Shoot;
    }
    void OnDisable()
    {
        _playerAction.Player.Look.performed -= CalculateDirection;
        _playerAction.Player.Look.canceled -= CalculateDirection;
        _playerAction.Player.Attack.performed -= Shoot;
        _playerAction.Player.Attack.canceled -= Shoot;
        _playerAction.Disable();
    }
    private void CalculateDirection(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        Direction.x = Mathf.Sign(MovementInput.x);
        Direction.y = Mathf.Sign(MovementInput.y);
    }
}