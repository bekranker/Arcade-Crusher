using UnityEngine;
using DG.Tweening;
public class OrderResource : MonoBehaviour, IObjectInteractable
{
    [SerializeField] private WorkManager _workManager;
    [SerializeField] private FoodType _foodType;
    [SerializeField, Range(0, 10)] private float _shakeSpeed;
    public void ExecuteInteraction()
    {
        _workManager.TakeResource(_foodType);
        DOTween.Kill(transform);
        transform.localScale = Vector2.one;
        transform.DOPunchScale(Vector3.one * _shakeSpeed, .3f);
    }

    public FoodType GetFood() => _foodType;
}