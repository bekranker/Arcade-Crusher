using System;
using UnityEngine;
public class MoveState : MonoBehaviour, IState
{
    [Header("----Movement Props")]
    [SerializeField, Range(0, 10)] float _speed;
    public Vector3 TargetPosition { get; set; }
    public Action AfterMove { get; set; }
    [Header("----Other Props")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Customer _customer;

    private bool _invokeOnce;
    /// <summary>
    /// NPC movement function. The NPC will be moving towards seat position
    /// </summary>
    /// <param name="targetPoint"></param>


    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        // Her frame'de hedef pozisyona doğru hareket et.
        if (transform.position != TargetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, _speed * Time.deltaTime);
        }
        else
        {
            if (!_invokeOnce)
            {
                // Hedefe ulaşıldığında AfterMove çağrılır.
                AfterMove?.Invoke();
            }
        }
    }
    void OnDestroy() => AfterMove = null;

    public void OnEnter()
    {
        _customer.ChangeState("Move");
        _invokeOnce = false;
    }
}