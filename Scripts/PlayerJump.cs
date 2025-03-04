using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerJump : MonoBehaviour
{
    [Header("---Jump Values")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _jumpValue;


    public Vector2 Direction;
    public Vector2 MovementInput;
    private Player_Actions _playerActions;

    void Awake()
    {
        _playerActions = new();
    }
    void OnEnable()
    {
        _playerActions.Enable();
        _playerActions.Player.Look.performed += CalculateDirection;
        _playerActions.Player.Look.canceled += CalculateDirection;
        _playerActions.Player.Attack.performed += JumpMe;
        _playerActions.Player.Attack.canceled += JumpMe;
    }
    void OnDisable()
    {
        _playerActions.Player.Look.performed -= CalculateDirection;
        _playerActions.Player.Look.canceled -= CalculateDirection;
        _playerActions.Player.Attack.performed -= JumpMe;
        _playerActions.Player.Attack.canceled -= JumpMe;
        _playerActions.Disable();
    }
    private void CalculateDirection(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector2>();
        MovementInput.y = Mathf.Sign(Direction.y);
    }
    public void JumpMe(InputAction.CallbackContext context)
    {
        if (Direction.y == 0)
        {
            _rb.linearVelocityX = -Direction.x * _jumpValue;
            return;
        }
        else
        {
            _rb.linearVelocityY = -Direction.y * _jumpValue;
            return;
        }
    }
}