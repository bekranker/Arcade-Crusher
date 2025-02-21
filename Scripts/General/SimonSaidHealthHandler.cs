using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimonSaidHealthHandler : MonoBehaviour
{
    [SerializeField] private List<Image> _heartImages = new();
    [SerializeField, Range(0, 100)] private float _health;
    [SerializeField] private GameObject _mighnessOneRect;
    private float _healthCount;
    private int _index;
    public static event Action OnDecrease, OnIncrease, OnDie;

    void Start()
    {
        Init();
    }
    public void Init()
    {
        _healthCount = _health;
        _index = 2;
        _mighnessOneRect.SetActive(false);
    }
    [Button]
    public void DecreaseHealth(float value)
    {
        if (_health < value)
        {
            Die();
            return;
        }
        _heartImages[_index].enabled = false;
        _index--;
        OnDecrease?.Invoke();
        TextEffect("-1");
        _healthCount -= value;
    }
    [Button]
    public void IncreaseHealth(float value)
    {
        if (_healthCount >= 3) return;
        TextEffect("+1");
        _index++;
        _heartImages[_index].enabled = true;
        OnIncrease?.Invoke();
        _healthCount += value;
    }
    private void TextEffect(string value)
    {
        _mighnessOneRect.SetActive(true);
        _mighnessOneRect.transform.localPosition = new Vector3(_heartImages[_index].transform.position.x, _mighnessOneRect.transform.localPosition.y, 0);
        _mighnessOneRect.GetComponent<TMP_Text>().text = value;
        _mighnessOneRect.transform.DOLocalMoveY(_mighnessOneRect.transform.localPosition.y + 10, .3f);
        _mighnessOneRect.GetComponent<TMP_Text>().DOFade(0, .3f);
    }
    public void Die()
    {
        OnDie?.Invoke();
    }
}