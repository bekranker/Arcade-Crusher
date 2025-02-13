using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
public class PlayerMovement : MonoBehaviour
{
    [Header("----Skateboard Components")]
    [SerializeField] private List<Transform> _wheels = new();

    [Header("----Movement Props")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField, Range(0, 100)] private float _speed;
    [SerializeField] private AnimationCurve _acceleration;
    [SerializeField] private AnimationCurve _decceleration;
    [Header("----Interaction Maskes")]
    [SerializeField] private LayerMask _layerMask;
    Player_Actions _playerActions;
    public Vector2 MovementInput;
    [Header("----Sprint Props")]
    [SerializeField, Range(0, 100)] private float _sprintSpeed; //Dash power
    [SerializeField, Range(0, 5)] private float _sprintCoolDown; // cool down for dash again
    [SerializeField, Range(0, 5)] private float _sprintDuration; //how many times pass while dashing
    private bool _isSprinting; //dont execute run function if it is dahsing.
    private bool _canSprint;
    float _elapsedTime = 0;
    float _direction;
    public Action AfterMoveEvent;
    bool _canMove;
    private Vector3 _pointToGo;
    private void Awake()
    {
        _playerActions = new();
        _canSprint = true;
    }

    private void OnEnable()
    {
        // Input Action'larını aktif et ve dinle
        _playerActions.Player.Enable();
        _playerActions.Player.Move.performed += Move;
        _playerActions.Player.Move.canceled += Move;
        _playerActions.Player.Sprint.performed += Sprint;
        _playerActions.Player.Point.performed += PointRun;

    }
    void Update()
    {
        Run();
        GoPoint();
    }
    private void OnDisable()
    {
        // Input Action'larını devre dışı bırak ve dinlemeyi bırak
        _playerActions.Player.Move.performed -= Move;
        _playerActions.Player.Move.canceled -= Move;
        _playerActions.Player.Sprint.performed -= Sprint;
        _playerActions.Player.Point.performed -= PointRun;
        _playerActions.Player.Disable();
    }
    void Sprint(InputAction.CallbackContext context)
    {
        if (MovementInput.x == 0) return;
        if (!_canSprint) return;
        if (_isSprinting) return;
        print("Sprinting");


        _rb.linearVelocityX = _direction * _sprintSpeed;
        Invoke(nameof(StopDash), _sprintDuration);
        Invoke(nameof(ResetDash), _sprintCoolDown);
        _isSprinting = true;
        _canSprint = false;

    }
    private void StopDash()
    {
        _isSprinting = false;
        _rb.linearVelocityX = 0;
    }
    private void ResetDash()
    {
        _canSprint = true;
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

    /// <summary>
    /// Physically movement with acceleration and deacceleration
    /// </summary>
    void Run()
    {
        if (_isSprinting) return;
        //Wheel Turn
        _wheels?.ForEach((wheel) =>
        {
            wheel.Rotate(Vector3.back * Mathf.Abs(_rb.linearVelocityX));
        });
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
    void PointRun(InputAction.CallbackContext context)
    {
        _pointToGo = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        AfterMoveInit(_pointToGo);
        _direction = Mathf.Sign(_pointToGo.x - transform.position.x);
        transform.localScale = new Vector3(_direction, transform.localScale.y, transform.localScale.z);
        _canMove = true;
    }
    void GoPoint()
    {
        if (!_canMove) return;
        _wheels?.ForEach((wheel) =>
            {
                wheel.Rotate(Vector3.back * _speed);
            });
        if (transform.position.x != _pointToGo.x)
        {
            _canMove = true;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_pointToGo.x, transform.position.y), _speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(_pointToGo.x, transform.position.y);
            AfterMoveEvent?.Invoke();
            AfterMoveEvent = null;
            _canMove = false;
        }
    }
    private void AfterMoveInit(Vector3 origin)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.forward, 100, _layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out IObjectInteractable interactableComponeny))
            {
                AfterMoveEvent = interactableComponeny.ExecuteInteraction;
            }
        }
    }
}