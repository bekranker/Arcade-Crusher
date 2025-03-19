using Sirenix.OdinInspector;
using UnityEngine;

public class SlotTimer : Slot
{
    [SerializeField] private FoodType _slotObject;
    [SerializeField] private float _delay;
    [SerializeField] private Slots _slot;

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
        Take(_delay, _slotObject);
    }
    public override void Take(float seconds, FoodType slotObject)
    {
        base.Take(seconds, slotObject);
    }
}