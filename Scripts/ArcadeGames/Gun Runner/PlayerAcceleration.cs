using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAcceleration : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private AnimationCurve _acceleration;
    [SerializeField] private AnimationCurve _decceleration;
    [SerializeField, Range(0, 100)] private float _speed;

    private bool _isSprinting; //dont execute run function if it is dahsing.
    float _direction;
    float _elapsedTime = 0;
    private Player_Actions _playerActions;

    public Vector2 MovementInput;
    void Awake()
    {
        _playerActions = new();
    }
    void Update()
    {
        Run();
    }
    void OnEnable()
    {
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
    /// <summary>
    /// Physically movement with acceleration and deacceleration
    /// </summary>
    void Run()
    {
        if (_isSprinting) return;
        //return if there is no input
        //input giderse decceleration ile duracak.
        if (MovementInput.x == 0)
        {
            _elapsedTime = 0;
            if (_rb.linearVelocityX != 0)
            {
                //dicrease the horizontal velocity until reach zero with direction coming from velocityX as clamped.
                _rb.linearVelocityX -= _decceleration.Evaluate(Time.deltaTime) * _speed * Mathf.Clamp(_rb.linearVelocityX, -1, 1);
            }
            return;
        }
        _elapsedTime += Time.deltaTime;
        //think that like this; how much time elapsed that we can reach our target speed. and our target speed is _Speed * Time.deltaTime * MovementInput.x
        float tempAcceleration = _acceleration.Evaluate(_elapsedTime) * _speed * _direction;
        _rb.linearVelocityX = tempAcceleration;
        //Turning
        transform.localScale = new Vector3(_direction, transform.localScale.y, transform.localScale.z);
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
}