using System.Collections;
using ArcadeCrusher.Player;
using UnityEngine;

public class TopDownMovement : BaseMovement
{
    private Vector2 _mousePos;
    [SerializeField] private ParticleSystem _boosterParticle;
    private bool _openParticle;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _aimMovementSpeed;
    [SerializeField] private PlayerShoot _playerShoot;
    private bool _dash = false;
    private Vector3 _previousOffTheScreenPos;
    void Update()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (_mousePos - (Vector2)transform.position).normalized;
        if (ArcadeCrusher.ArcadeCrusherCustom.OffTheScreen(transform, Camera.main) && (Mathf.Abs(transform.position.x) != Mathf.Abs(_previousOffTheScreenPos.x) && Mathf.Abs(transform.position.y) != Mathf.Abs(_previousOffTheScreenPos.y)))
        {
            Vector2 rightTopCorner = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            Vector3 targetPos = transform.position;
            if (Mathf.Abs(transform.position.y) > rightTopCorner.y)
            {
                print(transform.position.y);
                targetPos = new Vector3(transform.position.x, -transform.position.y, 0);
            }
            if (Mathf.Abs(transform.position.x) > rightTopCorner.x)
            {
                print(transform.position.x);
                targetPos = new Vector3(-transform.position.x, transform.position.y, 0);
            }
            _previousOffTheScreenPos = transform.position;
            transform.position = targetPos;
        }
        if (_dash) return;

        Run();
        ClampVelocity();
        transform.up = Vector3.Slerp(transform.up, direction, Time.deltaTime * 10 * _aimMovementSpeed);
    }
    public override void Run()
    {
        if (MovementInput == Vector2.zero)
        {
            _boosterParticle.Stop();
            _rb.linearVelocity = Vector2.zero;
            _openParticle = true;
            return;
        }
        if (_openParticle)
        {
            _boosterParticle.Play();
            _openParticle = false;
        }
        Vector2 direction = (_mousePos - (Vector2)transform.position).normalized;
        _rb.linearVelocity += direction * _speed * Time.deltaTime;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        _playerShoot.OnShoot += OnDash;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        _playerShoot.OnShoot -= OnDash;
    }
    private void OnDash()
    {
        StartCoroutine(DashIE());
    }
    private IEnumerator DashIE()
    {
        Vector2 direction = (transform.up).normalized;
        _dash = true;
        _rb.linearVelocity = Vector2.zero;
        _rb.linearVelocity += -direction * _dashForce;
        _boosterParticle.Stop();
        yield return new WaitForSeconds(0.2f);
        _boosterParticle.Play();
        _rb.linearVelocity = Vector2.zero;
        _dash = false;
    }
    private void ClampVelocity() => _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _maxSpeed);
    private float CustomSign(float value)
    {
        if (value < 0)
        {
            return -1;
        }
        else if (value > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}