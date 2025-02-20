using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;
using System;

public class GeneralScoreHandler : MonoBehaviour, ISingleton<GeneralScoreHandler>
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private float _punchScale;
    private float _scoreCounter;
    public CinemachineImpulseSource ImpulseSource;
    public GeneralScoreHandler Instance { get; set; }
    public event Action OnIncrease, OnDecrease;
    /// <summary>
    /// Increase the score
    /// </summary>
    /// <param name="score">amount of increasing</param>
    public void IncreaseScore(float score)
    {
        OnIncrease?.Invoke();
        _scoreCounter += score;
        ChangeText();
    }
    /// <summary>
    /// decrease the score
    /// </summary>
    /// <param name="score">amount of decresing</param>
    public void DecreaseScore(float score)
    {
        OnDecrease?.Invoke();
        _scoreCounter -= score;
        ChangeText();
    }
    /// <summary>
    /// Changing TMP_Text varibale with DO Punch Sclae Effect
    /// </summary>
    private void ChangeText()
    {
        ImpulseSource.GenerateImpulse();
        DOTween.Kill(_scoreText.transform);
        _scoreText.text = _scoreCounter.ToString();
        _scoreText.transform.DOPunchScale(Vector3.one * _punchScale, .2f);
    }
}