using UnityEngine;
using UnityEngine.InputSystem;

public class GenericPlayerMovement : BaseMovement
{
    public bool CanWalk; //dont execute run function if it is dahsing.
    float _direction;
    float _elapsedTime = 0;
    private Player_Actions _playerActions;
    public static GenericPlayerMovement Instance;
    public Vector2 MovementInput;
    void Awake()
    {
        CanWalk = true;
        _playerActions = new();
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
    /// Physically movement with acceleration and deceleration using AddForce
    /// </summary>
    public override void Run()
    {
        if (!CanWalk) return;

        // Input yoksa yavaşlat
        if (MovementInput.x == 0)
        {
            _elapsedTime = 0;
            if (Mathf.Abs(_rb.linearVelocityX) > 0.1f)
            {
                // Ters yönde bir kuvvet uygula (sürtünme etkisi yaratır)
                float decelForce = _decceleration.Evaluate(Time.deltaTime) * _speed * Mathf.Sign(_rb.linearVelocityX);
                _rb.AddForce(Vector2.left * decelForce, ForceMode2D.Impulse);
            }
            return;
        }

        _elapsedTime += Time.deltaTime;

        // Hedef hızımıza ulaşmak için geçen süreye göre kuvvet hesapla
        float accelerationForce = _acceleration.Evaluate(_elapsedTime) * _speed * MovementInput.x;

        // Kuvvet uygula
        _rb.AddForce(Vector2.right * accelerationForce, ForceMode2D.Force);

        // Maksimum hızı aşmasını önle
        _rb.linearVelocity = new Vector2(Mathf.Clamp(_rb.linearVelocityX, -_maxSpeed, _maxSpeed), _rb.linearVelocityY);

        // Yüzünü hareket yönüne çevir
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