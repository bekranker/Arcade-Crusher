using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class RunnerMovement : MonoBehaviour
{
    [Header("---Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Grounded _playerGrounded;
    [SerializeField] private Slider _slider;
    [SerializeField] private Slider _jetpackSlider;
    [SerializeField] private ProcuderalGenerator _procuderalGenerator;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private CinemachineCamera _cinemachine;
    [SerializeField] private RunnerGameManager _runnerManager;
    [Header("---Raycast props")]
    [SerializeField] private float _length;
    [SerializeField] private LayerMask _groundMask;
    [Header("---Speed & Jump props")]
    [SerializeField] private float _speedMultipilier;
    [SerializeField] private float _jumpValue;
    [SerializeField] private float _jumpDecreaseSpeed;
    [SerializeField] private float _jumpMultiplier;
    [SerializeField] private float _speedIncreaseMultiplier;
    [SerializeField] private float _maxSpeed;

    private float _currentJumpValue;
    private int _direction = 1;
    private RaycastHit2D _hit2D;
    private float _currentSpeed;
    public bool _loseJustOnes, _winJustOnes;

    void Awake()
    {
        _currentJumpValue = _jumpValue;
        _jetpackSlider.maxValue = _currentJumpValue;
        _currentSpeed = _speedMultipilier;
        _slider.maxValue = _runnerManager.CurrentLevel.Length;
    }
    private void Update()
    {
        // if (TouchCorner())
        // {
        //     _loseScreen.LoseGame();
        //     print("lose");
        //     return;
        // }
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
        if (_hit2D.collider != null)
        {
            _direction *= -1;
            if (_direction == -1)
            {
                _cinemachine.Target.TrackingTarget = null;
            }
            else
                _cinemachine.Target.TrackingTarget = transform;

        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector2.up * _currentJumpValue * 4, ForceMode2D.Impulse);
        }
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(Vector2.up * _currentJumpValue * _jumpMultiplier);
            _currentJumpValue -= Time.deltaTime * _jumpDecreaseSpeed;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            _rb.linearVelocityY /= 2;
        }
        if (_playerGrounded.IsGrounded())
        {
            _currentJumpValue = _jumpValue;
        }
        UpdateSlider();
        UpdatePlayerSlider();
    }
    private void UpdateSlider()
    {
        if (transform.position.x > 0)
            _slider.value = Mathf.Abs(transform.position.x);
    }
    private void UpdatePlayerSlider()
    {
        _jetpackSlider.value = _currentJumpValue;
    }
    private bool TouchCorner()
    {
        Vector2 min = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector2 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        return transform.position.x > max.x || transform.position.x < min.x;
    }
}
