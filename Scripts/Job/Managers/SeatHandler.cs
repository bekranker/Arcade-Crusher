using System.Collections.Generic;
using UnityEngine;

public class SeatHandler : MonoBehaviour
{

    [Tooltip("This is where customer seat")]
    [SerializeField] private List<Transform> _seatPoints;
    private Dictionary<Transform, bool> _emptySeats = new();

    void Start()
    {
        for (int i = 0; i < _seatPoints.Count; i++)
        {
            _emptySeats.Add(_seatPoints[i], true);
        }
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
    public void TakeASeat(Customer customer)
    {

    }
}