using Sirenix.OdinInspector;
using UnityEngine;

public class SlotNormal : Slot
{
    [SerializeField] private FoodType _slotObject;
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
        Take(0, _slotObject);
    }
    public override void Take(float seconds, FoodType slotObject)
    {
        if (_takedCount <= 0)
        {
            Debug.LogWarning("there is no more food Left");
            return;
        }
        if (WorkManager.Instance.IsHandFull())
        {
            Debug.LogWarning("Hand is full");
            return;
        }
        base.Take(seconds, slotObject);
    }
}