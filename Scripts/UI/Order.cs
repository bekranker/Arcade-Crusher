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
    [SerializeField, Range(0, 10)] private float _scaleDuration;
    private FoodType _foodType;
    private int _orderCount;

    void Start()
    {
        transform.localScale = Vector3.zero;
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

        transform.DOScale(Vector2.one, _scaleDuration);
        _orderImage.sprite = _foodType.Prefab.GetComponent<SpriteRenderer>().sprite;
    }
    private void ChangeText()
    {
        DOTween.Kill(transform);

        transform.DOScale(Vector2.one, _scaleDuration);
        _orderCountTMP.text = "X" + _orderCount;
    }
}