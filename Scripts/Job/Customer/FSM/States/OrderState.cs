using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderState : MonoBehaviour, IState, IObjectInteractable
{
    [Tooltip("NPC's Canvas")]
    [SerializeField] private GameObject _orderBox;
    [SerializeField] private Image _remainingTimeImage;
    [Header("----ORder Props")]
    [SerializeField] private float _orderTimeCounter;
    [Tooltip("NPC's Tooltip in the UI like X2 burger etc")]
    [SerializeField] private Order _orderUIPrefab;
    [Header("----Other Props")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Customer _customer;

    private Dictionary<FoodType, int> _myOrders;
    public Transform Seat { get; set; }


    private bool _startCounting;
    private float _counter;

    public void OrderMenu()
    {
        _myOrders = _customer.MyCustomerHandler.GetOrderData();

        _orderBox.SetActive(true);
        PrepeareOrder();
    }
    private void PrepeareOrder()
    {
        print("PreparingOrder");
        foreach (KeyValuePair<FoodType, int> orderItem in _myOrders)
        {
            Order tempOrderUIPrefab = Instantiate(_orderUIPrefab, _orderBox.transform);
            tempOrderUIPrefab.Init(orderItem.Key, orderItem.Value);
        }
        Invoke(nameof(StartCountDown), 0.5f);
    }
    private void StartCountDown()
    {
        _startCounting = true;
    }
    public void StopCountDown()
    {
        _startCounting = false;
    }
    public void OnEnter()
    {
        _counter = _orderTimeCounter;
        _customer.ChangeState("Order");
        OrderMenu();
        _customer.MoveState.AfterMove = () => { _customer.MoveState.AfterMove = null; gameObject.SetActive(false); };
    }

    public void OnExit()
    {
        _orderBox.SetActive(false);
        _remainingTimeImage.gameObject.SetActive(false);
    }

    public void OnUpdate()
    {
        if (_startCounting)
        {
            if (_counter <= 0)
            {
                CustomerLose();
                StopCountDown();
            }
            else
            {
                _counter -= Time.deltaTime;
                _remainingTimeImage.fillAmount = _counter / _orderTimeCounter;
            }
        }
    }
    private void CustomerLose()
    {
        _customer.MoveState.TargetPosition = new Vector3(12, 0.5f, 0);
        _customer.StateMachine.ChangeState(_customer.MoveState);
        _customer.SeatHandler.SetSeatEmpty(Seat);
        _customer.MoneyHandler.DecreaseMoney(_myOrders.Count * 10);
    }
    private void CustomerWin()
    {
        _customer.StateMachine.ChangeState(_customer.EatState);
    }
    private bool CheckMatchOrder()
    {
        foreach (FoodType handItem in _customer.WorkManager.Hand)
        {
            if (!_myOrders.ContainsKey(handItem)) return false;
        }
        return true;
    }
    public void ExecuteInteraction()
    {
        print("Interact with customer");
        //Change State to Eat
        if (CheckMatchOrder() && _counter > 0)
        {
            print("yummy yummy here");
            CustomerWin();
        }
        else
        {
            print("this is the biggest dog shit ever I seen in my life");
            _customer.MoveState.TargetPosition = new Vector3(12, 0.5f, 0);
            _customer.StateMachine.ChangeState(_customer.MoveState);
            _customer.MoneyHandler.DecreaseMoney(_myOrders.Count * 10);
            _customer.SeatHandler.WrongOrder(Seat);
        }
    }
}