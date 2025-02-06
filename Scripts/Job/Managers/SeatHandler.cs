using System.Collections.Generic;
using UnityEngine;

public class SeatHandler : MonoBehaviour
{

    [Header("---Components")]
    [SerializeField] private CustomerHandler _customerHandler;
    [Header("---Seat Props")]
    [Tooltip("This is where customer seat")]

    [SerializeField] private List<Transform> _seatPoints;
    [SerializeField] private WorkManager _workManager;
    public Dictionary<Transform, bool> EmptySeats = new();



    void Start()
    {
        for (int i = 0; i < _seatPoints.Count; i++)
        {
            EmptySeats.Add(_seatPoints[i], true);
        }
    }
    /// <summary>
    /// Checing is there have any empty seat to seat.
    /// </summary>
    /// <returns></returns>
    public bool AllSeatBusy()
    {
        foreach (KeyValuePair<Transform, bool> seat in EmptySeats)
        {
            if (seat.Value)
            {
                return false;
            }
        }
        return true;
    }
    public int EmptySeatCount()
    {
        int count = 0;
        foreach (KeyValuePair<Transform, bool> seat in EmptySeats)
        {
            if (seat.Value)
            {
                count++;
            }
        }

        return count;
    }
    public List<Transform> GetEmptySeats()
    {
        List<Transform> tempEmptySeats = new();

        // First, collect the keys of the empty seats
        foreach (KeyValuePair<Transform, bool> seat in EmptySeats)
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
        int randomSeat = Random.Range(0, tempEmptySeats.Count);
        EmptySeats[tempEmptySeats[randomSeat]] = false;
        return tempEmptySeats[randomSeat];
    }
    public void TakeASeat()
    {
        if (EmptySeatCount() <= 0) return;
        Customer tempCustomer = _customerHandler.GetFromLine();
        Transform targetSeat = TakeRandomSeat();
        tempCustomer.OrderState.Seat = targetSeat;
        tempCustomer.MoveState.TargetPosition = targetSeat.position;
        tempCustomer.MoveState.AfterMove = () => tempCustomer.StateMachine.ChangeState(tempCustomer.OrderState);
        tempCustomer.StateMachine.ChangeState(tempCustomer.MoveState);
    }
    public void SetSeatEmpty(Transform seat)
    {
        if (!EmptySeats.ContainsKey(seat)) return;
        EmptySeats[seat] = true;
    }
    public void WrongOrder(Transform seat)
    {
        if (!EmptySeats.ContainsKey(seat)) return;
        _workManager.ThrowToTrash();
        EmptySeats[seat] = true;
    }
}