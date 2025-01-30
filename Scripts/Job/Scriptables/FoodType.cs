using UnityEngine;

[CreateAssetMenu(fileName = "FoodType", menuName = "Scriptable Objects/Work/FoodType")]
public class FoodType : ScriptableObject
{
    public GameObject Prefab;
    public float Price;
}
