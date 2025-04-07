using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour
{
    public List<FoodType> SlotObjects = new();
    [SerializeField] private Image _slotImage;
    public int MaxTakeCount = 1;
    protected int _takedCount;

    void Start()
    {
        _takedCount = MaxTakeCount;
    }
    /// <summary>
    /// Adding to the Slot
    /// </summary>
    /// <param name="slotObject"></param>
    public virtual void Put(FoodType slotObject)
    {
        SlotObjects.Add(slotObject);
        _slotImage.sprite = SlotObjects[0].FoodSprite;
    }
    /// <summary>
    /// taking from machine to hand
    /// </summary>
    /// <param name="seconds">gived second to come hand</param>
    public virtual async void Take(float seconds, FoodType slotObject)
    {
        //if (!SlotObjects.Contains(slotObject)) return;
        await UniTask.Delay(System.TimeSpan.FromSeconds(seconds));
        WorkManager.Instance.TakeResource(slotObject);
        SlotObjects.Remove(slotObject);
        Debug.Log("Alindi");
    }
}