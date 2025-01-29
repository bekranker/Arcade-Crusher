using System.Collections.Generic;
using UnityEngine;

public abstract class CustomerHandler : MonoBehaviour
{
    [Tooltip("Food Types")]
    [SerializeField] private List<OrderObject> _orderObjects = new();
    [Tooltip("This is where customer seat")]
    [SerializeField] private List<Transform> _seatPoints;
    private Dictionary<Transform, bool> _emptySeats = new();

    /// <summary>
    /// returning Orders Length
    /// </summary>
    private int _ordersLength => _orderObjects.Count;


    /// <summary>
    /// returning random order list
    /// </summary>
    /// <returns>Order List</returns>
    public virtual List<OrderObject> GetOrderData()
    {
        int tempOrderObjectCount = Random.Range(0, _ordersLength);
        List<OrderObject> tempOrders = new();
        for (int i = 0; i < tempOrderObjectCount; i++)
        {
            tempOrders.Add(_orderObjects[Random.Range(0, _ordersLength)]);
        }
        return tempOrders;
    }
    /// <summary>
    /// Checing is there have any empty seat to seat.
    /// </summary>
    /// <returns></returns>
    public bool AllSeatBusy()
    {
        bool result = true;
        foreach (KeyValuePair<Transform, bool> seat in _emptySeats)
        {
            if (!seat.Value)
            {
                result = seat.Value;
            }
        }
        return result;
    }
    public List<Transform> GetEmptySeats()
    {
        List<Transform> tempEmptySeats = new();
        foreach (KeyValuePair<Transform, bool> seat in _emptySeats)
        {
            if (seat.Value)
            {
                tempEmptySeats.Add(seat.Key);
            }
        }
        return tempEmptySeats;
    }
    public Transform TakeRandomSeat()
    {
        if (AllSeatBusy())
        {
            Debug.Log("All Seats are busy !");
            return null;
        }
        List<Transform> tempEmptySeats = GetEmptySeats();
        return tempEmptySeats[Random.Range(0, tempEmptySeats.Count)];
    }

}