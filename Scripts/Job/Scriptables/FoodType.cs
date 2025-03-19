using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodType", menuName = "Scriptable Objects/Work/FoodType")]
public class FoodType : ScriptableObject, ISlotObject
{
    public Sprite FoodSprite;
    public float Price;
}
