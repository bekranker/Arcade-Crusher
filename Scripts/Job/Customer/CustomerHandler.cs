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
    [Tooltip("NPC Spawnpoint")]
    [SerializeField] private Transform _spawnPoint;
    [Tooltip("Customers line's start point")]
    [SerializeField] private Transform _linestartPoint;
    [Tooltip("Distance each customers")]
    [SerializeField, Range(0, 2)] private float _distance;
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
            currentCustomer.Move(new Vector3(_linestartPoint.transform.position.x + (_queueForEnter.Count * _distance), _linestartPoint.transform.position.y, 0));
            _queueForEnter.Enqueue(currentCustomer);
        }
    }
    /// <summary>
    /// Getting from queue to sit
    /// </summary>
    private void GetFromLine()
    {
        Customer nextCustomerToSeat = _queueForEnter.Dequeue();

    }
}