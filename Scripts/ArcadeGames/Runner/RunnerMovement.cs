using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ZilyanusLib.Audio;

public class RunnerMovement : MonoBehaviour
{
    [Header("---Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Grounded _playerGrounded;
    [SerializeField] private ProcuderalGenerator _procuderalGenerator;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private CinemachineCamera _cinemachine;
    [SerializeField] private RunnerGameManager _runnerManager;
    [Header("---Raycast props")]
    [SerializeField] private float _length;
    [SerializeField] private LayerMask _groundMask;
    [Header("---Speed & Jump props")]
    public float CurrentJumpValue;
    [SerializeField] private float _speedMultipilier;
    [SerializeField] private float _jumpValue;
    public float JumpDecreaseSpeed;
    [SerializeField] private float _jumpMultiplier;
    [SerializeField] private float _speedIncreaseMultiplier;
    [SerializeField] private float _maxSpeed;
    [Header("---UI")]
    [SerializeField] private Slider _slider;
    private bool _inited;
    private bool _didTouch;

    public event Action OnJumpStart, OnJumpEnd, OnJumpHolding;
    private int _direction = 1;
    private float _currentSpeed;
    public bool _loseJustOnes, _winJustOnes;
    private Player_Actions _inputActions;
    private RaycastHit2D _hit2D, _hit2DForSound;
    void Awake()
    {
        _inputActions = new();
        CurrentJumpValue = _jumpValue;
        _currentSpeed = _speedMultipilier;
        ChangeUIVisual();
    }
    void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.UFORunner.Jump.performed += JumpStart;
        _inputActions.UFORunner.Jump.canceled += JumpCancelled;
    }
    void OnDisable()
    {
        _inputActions.UFORunner.Jump.performed -= JumpStart;
        _inputActions.UFORunner.Jump.canceled -= JumpCancelled;
        _inputActions.Disable();
    }
    public void ChangeUIVisual()
    {
        _slider.maxValue = _runnerManager.CurrentLevel.Length;
    }
    private void Update()
    {
        if (_playerGrounded.IsGrounded() && !_inited)
        {
            _cinemachine.Target.TrackingTarget = transform;
            _inited = true;
        }
        JumpHolding();
        if (!_winJustOnes && transform.position.x >= _runnerManager.CurrentLevel.Length)
        {
            Debug.Log("win");
            _runnerManager.NextLevel();
            _winJustOnes = true;
            return;
        }
        transform.position += Vector3.right * _direction * Time.deltaTime * _currentSpeed;
        if (_currentSpeed < _maxSpeed)
        {
            _currentSpeed += Time.deltaTime * _speedIncreaseMultiplier;
        }
        _hit2D = Physics2D.Raycast(transform.position, Vector2.right * _direction, _length, _groundMask);
        _hit2DForSound = Physics2D.Raycast(transform.position, Vector2.left * _direction, _length + .4f, _groundMask);
        if (_hit2D.collider != null)
        {
            if (_hit2DForSound.collider == null)
            {
                AudioClass.PlayAudio("MiniGames/UFORunner/UFOWALL", .4f, "General", "Sound", 1, .3f);
            }
            _direction *= -1;
            transform.localScale = Vector2.one * _direction;
            if (_direction == -1)
            {
                _cinemachine.Target.TrackingTarget = null;
            }
            else
                _cinemachine.Target.TrackingTarget = transform;

        }
        if (_playerGrounded.IsGrounded())
        {
            if (!_didTouch)
            {
                _didTouch = true;
            }
            CurrentJumpValue = _jumpValue;
        }

        UpdateSlider();
    }
    private void JumpStart(InputAction.CallbackContext context)
    {
        if (CurrentJumpValue <= 0) return;
        _didTouch = false;
        OnJumpStart?.Invoke();
        _rb.AddForce(Vector2.up * CurrentJumpValue * 4, ForceMode2D.Impulse);
    }
    private void JumpHolding()
    {
        if (CurrentJumpValue <= 0) return;
        OnJumpHolding?.Invoke();
        if (_inputActions.UFORunner.Jump.ReadValue<float>() > 0)
        {
            print("holding");
            _rb.AddForce(Vector2.up * CurrentJumpValue * _jumpMultiplier);
            CurrentJumpValue -= Time.deltaTime * JumpDecreaseSpeed;
        }
    }
    private void JumpCancelled(InputAction.CallbackContext context)
    {
        OnJumpEnd?.Invoke();
        _rb.linearVelocityY /= 2;
    }
    private void UpdateSlider()
    {
        if (transform.position.x > 0)
            _slider.value = Mathf.Abs(transform.position.x);
    }
    public bool CanJump() => CurrentJumpValue > 0;
}
