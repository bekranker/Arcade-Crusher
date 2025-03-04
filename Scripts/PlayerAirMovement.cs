using UnityEngine;

public class PlayerAirMovement : BaseMovement
{
    public bool CanWalk;
    float _elapsedTime = 0;
    public static PlayerAirMovement Instance;
    void Awake()
    {
        CanWalk = true;
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
    public override void Run()
    {
        if (!CanWalk) return;
        if (_grounded.IsGrounded()) return;
        print("Air Movement");
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
}
