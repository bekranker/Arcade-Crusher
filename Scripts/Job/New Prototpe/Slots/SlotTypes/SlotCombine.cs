using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SlotCombine : Slot
{
    [SerializeField] private Slots _slot;

    public override void Put(FoodType slotObject)
    {
        if (!WorkManager.Instance.IsHandFull())
        {
            print("Hand is Empty");
            return;
        }
        Combine(slotObject);
    }
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
        print("Food Putted");
        if (WorkManager.Instance.Hand?[0] == null) return;
        Put(WorkManager.Instance.Hand?[0]);
    }

    public void Combine(ISlotObject slotObject)
    {
    }
}