using UnityEngine;
using UnityEngine.InputSystem;

public class BulletManager : MonoBehaviour
{
    [Header("---Bullet Props")]
    [SerializeField] private Transform _bulletPos;
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
        Bullet spawnedBullet = Instantiate(BulletPrefab, _bulletPos.position, Quaternion.identity);
        print(Direction);
        spawnedBullet.DirectionToGo = Direction;
    }
    void OnEnable()
    {
        _playerAction.Enable();
        _playerAction.Player.Look.performed += CalculateDirection;
        _playerAction.Player.Look.canceled += CalculateDirection;
        _playerAction.Player.Attack.performed += Shoot;
    }
    void OnDisable()
    {
        _playerAction.Player.Look.performed -= CalculateDirection;
        _playerAction.Player.Look.canceled -= CalculateDirection;
        _playerAction.Player.Attack.performed -= Shoot;
        _playerAction.Disable();
    }
    private void CalculateDirection(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        Direction.x = Sign(MovementInput.x);
        Direction.y = Sign(MovementInput.y);
    }
    public static float Sign(float f)
    {
        if (f == 0)
        {
            return 0;
        }
        return (f > 0f) ? 1f : (-1f);
    }
}