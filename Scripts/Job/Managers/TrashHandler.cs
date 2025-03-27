using DG.Tweening;
using UnityEngine;

public class TrashHandler : MonoBehaviour, IObjectInteractable
{
    [SerializeField] private WorkManager _workManager;

    public void ExecuteInteraction()
    {
        _workManager.ClearHand();
        DoTweenEffect();
    }
    private void DoTweenEffect()
    {
        DOTween.Kill(transform);
        transform.localScale = Vector2.one;
        transform.DOPunchScale(DoTweenProps.Instance.PunchScale_Slot, DoTweenProps.Instance.Delay_SlotDelay);
    }
}
