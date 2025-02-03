using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyTMP;
    [SerializeField, Range(0, 10)] private float _earningEffectSize;

    private float _vault;

    public void IncreaseMoney(float count)
    {
        _vault += count;
        _moneyTMP.text = _vault.ToString() + "$";
        TextEffect();
    }
    public void DecreaseMoney(float count)
    {
        _vault += count;
        _moneyTMP.text = _vault.ToString() + "$";
        TextEffect();
    }
    private void TextEffect()
    {
        DOTween.Kill(_moneyTMP.transform);
        _moneyTMP.transform.DOPunchScale(Vector3.one * _earningEffectSize, .3f).OnComplete(() => { _moneyTMP.transform.localScale = Vector3.one; });
    }
}