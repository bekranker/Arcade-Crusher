using UnityEngine;

public class IdleState : MonoBehaviour, IState
{

    [Header("----Other Props")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Customer _customer;
    public void OnEnter()
    {
        _customer.ChangeState("Idle");
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}