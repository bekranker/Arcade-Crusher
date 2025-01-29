using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorkManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private List<OrderResource> _orderResources;

    public event Action HandAction, TrashAction;

    public bool HasPlate;
    public List<FoodType> _hand;


    public void TakeResource(FoodType foodType)
    {
        _hand.Add(foodType);
        //UI change or plate reposition for the objects
        HandAction?.Invoke();
    }
    public void TakePlate() => HasPlate = true;
    /// <summary>
    /// I think this function throwing to trash 💀
    /// </summary>
    public void ThrowToTrash()
    {
        HasPlate = false;
        TrashAction?.Invoke();
        _hand.Clear();
    }
}