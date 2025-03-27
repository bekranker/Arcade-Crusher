using DG.Tweening;
using UnityEngine;
public class SlotOne : Slots, IObjectInteractOne
{
    public void Execute()
    {
        InvokeOnSlotAction();
        print("Slot One");
    }
}
