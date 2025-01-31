using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomerHandler : MonoBehaviour
{
    [Tooltip("Food Types")]
    [SerializeField] private List<FoodType> _FoodTypes = new();
    [Header("---Customer Props")]
    [Tooltip("Maximum number getting same food for a customer")]
    [SerializeField] private int _maxSameFoodCount;
    [Tooltip("Customer Types for randomize")]
    [SerializeField] private List<CustomerType> MyCustomerTypes = new();
    [Tooltip("Customer Prefab to Instiante")]
    [SerializeField] private Customer _customer;
    [SerializeField] private SeatHandler _seatHandler;

    [Header("---Spawner Props")]
    [Tooltip("NPC Spawnpoint")]
    [SerializeField] private Transform _spawnPoint;
    [Tooltip("Customers line's start point")]
    [SerializeField] private Transform _linestartPoint;
    [Tooltip("Distance each customers")]
    [SerializeField, Range(0, 2)] private float _distance;
    public Queue<Customer> _queueForEnter = new();

    // returning Orders Length
    private int _ordersLength => _FoodTypes.Count;
    private Player_Actions _playerActions;
    void Awake()
    {
        _playerActions = new();

    }
    void OnEnable()
    {
        _playerActions.Player.Enable();
        _playerActions.Player.Debug.performed += CreateCustomer;
    }
    void OnDisable()
    {
        _playerActions.Player.Debug.performed -= CreateCustomer;
        _playerActions.Player.Disable();
    }

    /// <summary>
    /// this function is for start
    /// </summary>
    public void CreateCustomer(InputAction.CallbackContext context)
    {
        AddToLine(EmptySeatsCount());
    }

    /// <summary>
    /// Checking the empty seat count
    /// </summary>
    /// <returns></returns>
    private int EmptySeatsCount() => _seatHandler.GetEmptySeats().Count;

    /// <summary>
    /// Creating new customers to add the Queue<Customer>
    /// </summary>
    /// <param name="count">Count of the Customers will be create</param>
    private void AddToLine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Customer currentCustomer = Instantiate(_customer, _spawnPoint.position, Quaternion.identity);
            currentCustomer.Init(this, MyCustomerTypes[Random.Range(0, MyCustomerTypes.Count)]);
            currentCustomer.MyStateMachine.ChangeState(currentCustomer.MyGetInLineState);
            _queueForEnter.Enqueue(currentCustomer);
        }
    }
    public void Move(Customer customer)
    {
        customer.Move(new Vector3(_linestartPoint.transform.position.x + (_queueForEnter.Count * _distance), _linestartPoint.transform.position.y, 0));
    }
    /// <summary>
    /// returning random order list
    /// </summary>
    /// <returns>Order List</returns>
    public Dictionary<FoodType, int> GetOrderData()
    {
        int tempFoodTypeCount = Random.Range(0, _ordersLength);
        Dictionary<FoodType, int> tempOrders = new();
        for (int i = 0; i < tempFoodTypeCount; i++)
        {
            int randomFood = Random.Range(0, _ordersLength);
            if (tempOrders.ContainsKey(_FoodTypes[randomFood]))
            {
                if (tempOrders[_FoodTypes[randomFood]] <= _maxSameFoodCount)
                {
                    tempOrders[_FoodTypes[randomFood]]++;
                }
            }
            else
                tempOrders.Add(_FoodTypes[randomFood], 1);
        }
        return tempOrders;
    }

    /// <summary>
    /// Getting from queue to sit
    /// </summary>
    private void GetFromLine()
    {
        Customer nextCustomerToSeat = _queueForEnter.Dequeue();

    }
}