using UnityEngine;

public class Fry : MonoBehaviour
{
    // Example properties
    public string FryType { get; set; }
    public float CookingTime { get; set; }
    [SerializeField] private SlotTimer _timerSlot;
    private FoodType _cookedFood;
    private bool _cooked;
    void OnEnable()
    {
        _timerSlot.OnCooked += Cook;
    }
    void OnDisable()
    {
        _timerSlot.OnCooked -= Cook;
    }
    // Example method
    private async void Cook()
    {
        if (_cooked)
        {
            Debug.Log("Cooked");
            _timerSlot.Take(0, _cookedFood);
            return;
        }
        else
        {
            if (_timerSlot.SlotObjects.Count == 0)
            {
                _timerSlot.Put(WorkManager.Instance.Hand[0]);
                WorkManager.Instance.ClearHand();
                return;
            }
            _cookedFood = await WorkManager.Instance.Cook(_timerSlot.SlotObjects[0]);
            if (_cookedFood == null)
            {
                Debug.LogError("Cooking Failed");
                return;
            }
            _cooked = true;
            _timerSlot.SlotObjects.Clear();
            _timerSlot.Put(_cookedFood);
            Debug.Log("Cooking");
        }
    }
}