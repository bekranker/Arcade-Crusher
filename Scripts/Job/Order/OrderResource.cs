using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
public class OrderResource : MonoBehaviour, IObjectInteractable, IObjectInteractableNearBy
{
    [SerializeField] private List<GameObject> _ui;
    [SerializeField, Range(0, 10)] private float _shakeSpeed;
    public event Action OnInteraction;
    private bool _nearByInteraction;

    void Start()
    {
        _nearByInteraction = true;
        _ui?.ForEach((ui) => ui.SetActive(false));
    }
    public void ExecuteInteraction()
    {
        DOTween.Kill(transform);
        transform.localScale = Vector2.one;
        transform.DOPunchScale(DoTweenProps.Instance.PunchScale_Slot, DoTweenProps.Instance.Delay_SlotDelay);
        OnInteraction?.Invoke();
    }
    public void ExecuteNearInteraction()
    {
        if (!_nearByInteraction) return;
        _ui?.ForEach((ui) =>
        {
            ui.SetActive(true);
        });
        _nearByInteraction = false;
        Debug.Log("Near By");
    }

    public void ExitArea()
    {
        _nearByInteraction = true;
        _ui?.ForEach((ui) =>
        {
            ui.SetActive(false);
        });
        Debug.Log("Exited from area");
    }
}