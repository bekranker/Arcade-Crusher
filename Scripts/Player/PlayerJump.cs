using ArcadeCrusher;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerJump : MonoBehaviour
{
    [Header("---Jump Values")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Vector2 _jumpValue;


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
        MovementInput.y = ArcadeCrusherCustom.ArcadeCrusherMath.Sign(-Direction.y);
        MovementInput.x = ArcadeCrusherCustom.ArcadeCrusherMath.Sign(-Direction.x);
    }
    public void JumpMe(InputAction.CallbackContext context)
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(MovementInput * _jumpValue * 10, ForceMode2D.Impulse);
        return;
    }
}