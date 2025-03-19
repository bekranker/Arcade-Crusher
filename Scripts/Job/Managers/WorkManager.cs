using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    public List<MerchFoods> MerchFoods = new();
    public List<FoodType> Hand = new();
    [SerializeField] private Image _handUI;
    public static WorkManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Taking something
    /// </summary>
    /// <param name="foodType"></param>
    public void TakeResource(FoodType foodType)
    {
        if (Hand.Count != 0) return;
        _handUI.sprite = foodType.FoodSprite;
        Hand.Add(foodType);
        Debug.Log("Aldim");
    }
    /// <summary>
    /// I think this function throwing to trash 💀
    /// </summary>
    public void ThrowToTrash()
    {
        Hand.Clear();
    }
    public bool IsHandFull() => Hand?.Count > 0;
}