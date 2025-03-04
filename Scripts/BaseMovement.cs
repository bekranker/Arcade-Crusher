using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Grounded))]
public abstract class BaseMovement : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected AnimationCurve _acceleration;
    [SerializeField] protected AnimationCurve _decceleration;
    [SerializeField, Range(0, 100)] protected float _speed;
    [SerializeField, Range(-100, 100)] protected float _maxSpeed;
    [SerializeField] protected Grounded _grounded;
    [SerializeField] protected float _direction;
    private Player_Actions _playerActions;

    public Vector2 MovementInput;

    void Awake()
    {
        print("mk");
    }
    /// <summary>
    /// we are reading directions from player input
    /// </summary>
    /// <param name="context">it is returning new input system paramteres</param>
    void Move(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        _direction = Mathf.Sign(MovementInput.x);
    }
    public abstract void Run();

    void OnEnable()
    {
        _playerActions = new Player_Actions();
        _playerActions.Player.Enable();

        _playerActions.Player.Move.performed += Move;
        _playerActions.Player.Move.canceled += Move;
    }
    void OnDisable()
    {
        _playerActions.Player.Move.performed -= Move;
        _playerActions.Player.Move.canceled -= Move;
        _playerActions.Player.Disable();

    }
}