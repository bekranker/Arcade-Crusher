
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatState : MonoBehaviour, IState
{
    [Header("----Other Props")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Customer _customer;

    public Transform Seat { get; set; }
    private SeatHandler _seatHandler;


    public void OnEnter()
    {
        _customer.ChangeState("Eating");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
    }
}