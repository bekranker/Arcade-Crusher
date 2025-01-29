using UnityEngine;
using DG.Tweening;
public class OrderResource : WorkManager, IObjectInteractable
{
    [SerializeField] private FoodType _foodType;
    [SerializeField, Range(0, 10)] private float _shakeSpeed;
    [SerializeField] private WorkManager _workManager;
    public void ExecuteInteraction()
    {
        transform.DOPunchScale(Vector3.one * _shakeSpeed, .3f);
        _hand.Add(_foodType);
        Debug.Log("sa");

    }

    public FoodType GetFood() => _foodType;


}