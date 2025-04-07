using UnityEngine;

[CreateAssetMenu(fileName = "FryScriptables", menuName = "Scriptables/FryScriptables", order = 1)]
public class FryScriptables : ScriptableObject
{
    public FoodType RawFood;
    public FoodType CookedFood;
    public float CookingTime;
}