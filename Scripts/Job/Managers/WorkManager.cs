using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] public GameObject Tray;
    public static event Action HandAction;
    public List<FoodType> Hand;
    public List<GameObject> HandGameObjects;


    /// <summary>
    /// Taking something
    /// </summary>
    /// <param name="foodType"></param>
    public void TakeResource(FoodType foodType)
    {
        if (!Tray.activeSelf) return;
        Debug.Log("Aldim");
        GameObject tempCreatedFood = Instantiate(foodType.Prefab);
        Hand.Add(foodType);
        HandGameObjects.Add(tempCreatedFood);
        tempCreatedFood.transform.SetParent(Tray.transform);
        //UI change or plate reposition for the objects
        HandAction?.Invoke();
    }
    public void TakePlate()
    {
        Tray.SetActive(true);
    }
    /// <summary>
    /// I think this function throwing to trash 💀
    /// </summary>
    public void ThrowToTrash()
    {
        for (int i = 0; i < HandGameObjects.Count; i++)
        {
            Destroy(HandGameObjects[i]);
        }
        Hand.Clear();
        HandGameObjects.Clear();
        Tray.SetActive(false);
    }
}