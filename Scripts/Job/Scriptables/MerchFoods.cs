using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Merch", menuName = "Scriptable Objects/Work/Merchs")]
public class MerchFoods : ScriptableObject
{
    public List<FoodType> MerchableFoods = new();
    public FoodType CreatedItem;
    public float CookingTime;

}