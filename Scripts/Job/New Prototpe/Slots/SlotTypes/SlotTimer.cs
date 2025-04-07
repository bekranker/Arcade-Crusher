using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SlotTimer : Slot
{
    [SerializeField] private Slots _slot;
    private FoodType _cookedFood;
    private bool _cooked;
    public event Action OnCooked;
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
        TimerAction();
    }
    private void TimerAction()
    {
        OnCooked?.Invoke();
    }
}