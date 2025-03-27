using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WorkManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    public List<MerchFoods> MerchFoods = new();
    public List<FoodType> Hand = new();
    [SerializeField] private Image _handUI;
    [SerializeField] private GameObject _handUIParent;
    public static WorkManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _handUIParent.SetActive(false);
    }
    /// <summary>
    /// Taking something
    /// </summary>
    /// <param name="foodType"></param>
    public void TakeResource(FoodType foodType)
    {
        if (Hand.Count != 0) return;
        _handUIParent.SetActive(true);
        HandUIEffect();
        _handUI.sprite = foodType.FoodSprite;
        Debug.Log("Aldim");
        Hand.Add(foodType);
    }
    public FoodType MetchResource(FoodType foodType)
    {
        FoodType selectedFoodType = null;
        foreach (MerchFoods merchs in MerchFoods)
        {
            if (merchs.MerchableFoods.Contains(Hand[0]) && merchs.MerchableFoods.Contains(foodType))
            {
                selectedFoodType = merchs.CreatedItem;
                break;
            }
        }
        return selectedFoodType;
    }
    private void HandUIEffect()
    {
        DOTween.Kill(_handUIParent.transform);
        _handUIParent.transform.DOPunchScale(DoTweenProps.Instance.PunchScale_PlayerUI, DoTweenProps.Instance.Delay_PlayerUI);
    }
    /// <summary>
    /// I think this function throwing to trash 💀
    /// </summary>
    public void ClearHand()
    {
        Hand.Clear();
        _handUIParent.SetActive(false);
    }
    public bool IsHandFull() => Hand?.Count > 0;

    public async UniTask<FoodType> Cook(FoodType foodType, float cookTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(cookTime));
        return MetchResource(foodType);
    }
}