using UnityEngine;
public abstract class BaseMovement : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected AnimationCurve _acceleration;
    [SerializeField] protected AnimationCurve _decceleration;
    [SerializeField, Range(0, 100)] protected float _speed;
    [SerializeField, Range(-100, 100)] protected float _maxSpeed;

    public abstract void Run();
}