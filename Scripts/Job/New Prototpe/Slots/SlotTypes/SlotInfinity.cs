using Sirenix.OdinInspector;
using UnityEngine;


public class SlotInfinity : Slot
{
    [SerializeField, PolymorphicDrawerSettings] private IBaseInventory _baseInventory;
    [SerializeField] private Slots _slot;
    [SerializeField] private FoodType _food;
    void OnEnable()
    {
        _slot.OnSlotAction += ExecuteAction;
    }
    void OnDisable()
    {
        _slot.OnSlotAction -= ExecuteAction;
    }
    private void ExecuteAction()
    {
        print("food taken");
        Take(0, _food);
    }
    public override void Take(float seconds, FoodType slotObject)
    {
        base.Take(seconds, slotObject);
    }
}