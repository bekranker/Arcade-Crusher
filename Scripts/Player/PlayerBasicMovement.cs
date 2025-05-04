using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Grounded))]
public class PlayerBasicMovement : BaseMovement
{
    void Update()
    {
        Run();
    }
    public override void Run()
    {
        if (MovementInput.x == 0)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocityY);
            return;
        }
        transform.localScale = new Vector3(_direction, transform.localScale.y, transform.localScale.z);

        _rb.linearVelocity = new Vector2(MovementInput.x * _speed, _rb.linearVelocityY);
        _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _maxSpeed);
    }
}