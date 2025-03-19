using System;

public class SlotTwo : Slots, IObjectInteractTwo
{
    public void Execute()
    {
        InvokeOnSlotAction();
        print("Slot Two");
    }
}
