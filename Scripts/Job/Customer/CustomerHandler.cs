using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{
    [Header("---Customer Props")]
    [Tooltip("Customer Types for randomize")]
    [SerializeField] private List<CustomerType> _customerTypes = new();
    [Tooltip("Customer Prefab to Instiante")]
    [SerializeField] private Customer _customer;
    [SerializeField] private SeatHandler _seatHandler;

    [Header("---Spawner Props")]
    [SerializeField] private Transform _spawnPoint;

    private Queue<Customer> _queueForEnter = new();

    /// <summary>
    /// this function is for start
    /// </summary>
    public void CreateCustomer()
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
            currentCustomer.Init(_seatHandler, _customerTypes[Random.Range(0, _customerTypes.Count)]);
            _queueForEnter.Enqueue(currentCustomer);
        }
    }
    private void GetFromLine()
    {
        Customer nextCustomerToSeat = _queueForEnter.Dequeue();
    }
}