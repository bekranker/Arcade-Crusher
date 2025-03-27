using Cysharp.Threading.Tasks;
using UnityEngine;

public class SlotTimer : Slot
{
    [SerializeField] private float _delay;
    [SerializeField] private Slots _slot;
    private FoodType _cookedFood;
    private bool _cooked;
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
        if (SlotObjects.Count != 0) return;
        if (_cooked)
        {
            Take(_delay, _cookedFood);
        }
        else
        {
            CookingAction();
        }
    }
    private async void CookingAction()
    {
        if (SlotObjects.Count == 0)
        {
            Put(WorkManager.Instance.Hand[0]);
            WorkManager.Instance.ClearHand();
            return;
        }
        _cookedFood = await WorkManager.Instance.Cook(SlotObjects[0], _delay);
        if (_cookedFood == null)
        {
            Debug.LogError("Cooking Failed");
            return;
        }
        _cooked = true;
        SlotObjects.Clear();
        Put(_cookedFood);

    }
}