using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class Order : MonoBehaviour
{
    [Header("---IMPORTANT PROPS")]
    [SerializeField] private Image _orderImage;
    [SerializeField] private TMP_Text _orderCountTMP;


    [Header("---Dotween Props")]
    [SerializeField, Range(0, 10)] private float _pucnhScale;
    [SerializeField, Range(0, 10)] private float _punchDuration;
    private Vector3 _startScale;
    private FoodType _foodType;
    private int _orderCount;

    void Start()
    {
        _startScale = transform.localScale;
    }
    public void Init(FoodType foodType, int orderCount)
    {
        _orderCount = orderCount;
        _foodType = foodType;
        ChangeImage();
        ChangeText();
    }
    private void ChangeImage()
    {
        DOTween.Kill(transform);
        transform.localScale = _startScale;

        transform.DOPunchScale(_pucnhScale * _startScale, _punchDuration);
        _orderImage.sprite = _foodType.Prefab.GetComponent<SpriteRenderer>().sprite;
    }
    private void ChangeText()
    {
        DOTween.Kill(transform);
        transform.localScale = _startScale;

        transform.DOPunchScale(_pucnhScale * Vector3.one, _punchDuration);
        _orderCountTMP.text = "X" + _orderCount;
    }
}