using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralHearthManager : MonoBehaviour
{
    [SerializeField] private int _hearthCount = 3;
    [SerializeField] private GameObject[] _hearths;
    [SerializeField] private LoseScreen _loseScreen;

    public static GeneralHearthManager Instance { get; private set; }
    public event Action OnHit;
    void Awake()
    {
        Instance = this;
    }
    public void DecreaseHealth()
    {
        OnHit?.Invoke();
        _hearthCount--;
        if (_hearthCount <= 0)
        {
            _loseScreen.LoseGame();
        }
        StartCoroutine(DamageIE());
        _hearths[_hearthCount].SetActive(false);
    }
    public void IncreaseHeatlh()
    {
        if (_hearthCount < _hearths.Length)
        {
            _hearths[_hearthCount].SetActive(true);
            _hearthCount++;
        }
    }
    private IEnumerator DamageIE()
    {
        MiniGameController.Instance.PauseTheGame();
        yield return new WaitForSecondsRealtime(1);
        MiniGameController.Instance.ContunieToPlay();
    }
}