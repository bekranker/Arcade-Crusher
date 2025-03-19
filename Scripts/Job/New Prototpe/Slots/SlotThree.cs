public class SlotThree : Slots, IObjectInteractThree
{
    public void Execute()
    {
        InvokeOnSlotAction();
        print("Slot Three");
    }
}
