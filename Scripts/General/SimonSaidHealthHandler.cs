using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SimonSaidHealthHandler : MonoBehaviour
{
    [SerializeField] private List<Image> _heartImages = new();
    [SerializeField, Range(0, 100)] private float _health;
    private float _healthCount;
    private int _index;
    public event Action OnDecrease, OnIncrease, OnDie;

    public void Init()
    {
        _healthCount = _health;
        _index = 2;
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
        _healthCount -= value;
    }
    [Button]
    public void IncreaseHealth(float value)
    {
        if (_healthCount >= 3) return;
        _index++;
        _heartImages[_index].enabled = true;
        OnIncrease?.Invoke();
        _healthCount += value;
    }
    public void Die()
    {
        OnDie?.Invoke();
    }
}