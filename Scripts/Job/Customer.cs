using System.Collections.Generic;
using UnityEngine;

public class Customer : CustomerHandler
{

    public override bool AllSeatBusy()
    {
        return base.AllSeatBusy();
    }
    public override List<Transform> GetEmptySeats()
    {
        return base.GetEmptySeats();
    }
    public override Transform TakeRandomSeat()
    {
        return base.TakeRandomSeat();
    }
}