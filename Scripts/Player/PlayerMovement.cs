using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [Header("----Skateboard Components")]
    [SerializeField] private List<Transform> _wheels = new();

    [Header("----Movement Props")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField, Range(0, 100)] private float _speed;
    [SerializeField] private AnimationCurve _acceleration;
    [SerializeField] private AnimationCurve _decceleration;
    Player_Actions _playerActions;
    public Vector2 MovementInput;
    float _elapsedTime = 0;



    private void Awake()
    {
        _playerActions = new();
    }

    private void OnEnable()
    {
        // Input Action'larını aktif et ve dinle
        _playerActions.Player.Enable();
        _playerActions.Player.Move.performed += Move;
        _playerActions.Player.Move.canceled += Move;
    }
    void Update()
    {
        Run();
    }
    private void OnDisable()
    {
        // Input Action'larını devre dışı bırak ve dinlemeyi bırak
        _playerActions.Player.Move.performed -= Move;
        _playerActions.Player.Move.canceled -= Move;
        _playerActions.Player.Disable();
    }
    /// <summary>
    /// we are reading directions from player input
    /// </summary>
    /// <param name="context">it is returning new input system paramteres</param>
    void Move(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
    /// <summary>
    /// Physically movement with acceleration and deacceleration
    /// </summary>
    void Run()
    {
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
        float tempAcceleration = _acceleration.Evaluate(_elapsedTime) * _speed * MovementInput.x;
        _rb.linearVelocityX = tempAcceleration;
        //Turning
        transform.localScale = new Vector3(MovementInput.x, transform.localScale.y, transform.localScale.z);

    }

}