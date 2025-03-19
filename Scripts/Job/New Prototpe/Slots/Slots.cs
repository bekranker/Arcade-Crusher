using System;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public abstract class Slots : MonoBehaviour
{
    [SerializeField] private float _shakeSpeed;
    public event Action OnSlotAction;
    protected void InvokeOnSlotAction()
    {
        DoTweenEffect();
        OnSlotAction?.Invoke();
    }
    private void DoTweenEffect()
    {
        DOTween.Kill(transform);
        transform.localScale = Vector2.one;
        transform.DOPunchScale(Vector3.one * _shakeSpeed, .3f);
    }
}
