
using UnityEngine;

public class EatState : MonoBehaviour, IState
{
    [Header("----Other Props")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Customer _customer;
    [SerializeField, Range(0, 10)] float _eatingDelay;

    public Transform Seat { get; set; }
    private SeatHandler _seatHandler;


    public void OnEnter()
    {
        _customer.ChangeState("Eating");
        Invoke(nameof(GiveTip), _eatingDelay);
    }
    private void GiveTip()
    {
        _customer.MoveState.TargetPosition = new Vector3(12, .5f, 0);
        _customer.MoveState.AfterMove = () => { gameObject.SetActive(false); };
        _customer.StateMachine.ChangeState(_customer.MoveState);
    }
    public void OnExit()
    {
        //leave some dishes
        //leave some tip
        //shoot money sound effect
        //shoot money UI effect
    }

    public void OnUpdate()
    {
    }
}