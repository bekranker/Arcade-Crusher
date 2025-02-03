using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    [SerializeField] private WorkManager _workManager;

    [Header("---Spawner Props")]
    [Tooltip("NPC Spawnpoint")]
    [SerializeField] private Transform _spawnPoint;
    [Tooltip("Customers line's start point")]
    [SerializeField] private Transform _linestartPoint;
    [Tooltip("Distance each customers")]
    [SerializeField, Range(0, 2)] private float _distance;
    public Queue<Customer> _queueForEnter = new();

    // returning Orders Length
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
        AddToLine(2);
    }

    /// <summary>
    /// Checking the empty seat count
    /// </summary>
    /// <returns></returns>


    /// <summary>
    /// Creating new customers to add the Queue<Customer>
    /// </summary>
    /// <param name="count">Count of the Customers will be create</param>
    private async void AddToLine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Customer currentCustomer = Instantiate(_customer, _spawnPoint.position, Quaternion.identity);

            currentCustomer.Init(this, MyCustomerTypes[Random.Range(0, MyCustomerTypes.Count)], _workManager, _seatHandler);

            currentCustomer.MoveState.TargetPosition = LinePoint();
            currentCustomer.MoveState.AfterMove = () =>
            {
                _seatHandler.TakeASeat();
            };
            currentCustomer.MyStateMachine.ChangeState(currentCustomer.MoveState);

            _queueForEnter.Enqueue(currentCustomer);

            await UniTask.Delay(500);
        }
    }

    public Vector3 LinePoint() => new Vector3(_linestartPoint.transform.position.x + (_queueForEnter.Count * _distance), _linestartPoint.transform.position.y, 0);
    /// <summary>
    /// returning random order list
    /// </summary>
    /// <returns>Order List</returns>
    public Dictionary<FoodType, int> GetOrderData()
    {
        int tempFoodTypeCount = Random.Range(0, _FoodTypes.Count);
        Dictionary<FoodType, int> tempOrders = new();
        for (int i = 0; i < tempFoodTypeCount; i++)
        {
            int randomFood = Random.Range(0, _FoodTypes.Count);
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
    public Customer GetFromLine()
    {
        Customer nextCustomerToSeat = _queueForEnter.Dequeue();
        return nextCustomerToSeat;
    }
}