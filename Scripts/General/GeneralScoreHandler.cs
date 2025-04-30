using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GeneralScoreHandler : MonoBehaviour, ISingleton<GeneralScoreHandler>
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private float _punchScale;
    public float ScoreCounter { get; set; }
    public CinemachineImpulseSource ImpulseSource;
    public static GeneralScoreHandler Instance { get; set; }
    public static event Action OnIncrease, OnDecrease;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        ChangeText();
    }

    /// <summary>
    /// Increase the score
    /// </summary>
    /// <param name="score">amount of increasing</param>
    public void IncreaseScore(float score)
    {
        StartCoroutine(IncreaseScoreIE(score));
    }
    public void DecreaseScore(float score)
    {
        StartCoroutine(DecreaseScoreIE(score));
    }
    public IEnumerator IncreaseScoreIE(float score)
    {
        float targetScore = ScoreCounter + score;
        while (ScoreCounter < targetScore)
        {
            ScoreCounter += 1;
            yield return new WaitForSeconds(.002f);
            ChangeText();
        }
        OnIncrease?.Invoke();
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// decrease the score
    /// </summary>
    /// <param name="score">amount of decresing</param>
    public IEnumerator DecreaseScoreIE(float score)
    {
        OnDecrease?.Invoke();
        float targetScore = ScoreCounter - score;
        while (ScoreCounter < targetScore)
        {
            ScoreCounter -= 1;
            yield return new WaitForSeconds(.1f);
            ChangeText();
        }
        OnDecrease?.Invoke();
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// Changing TMP_Text varibale with DO Punch Scale Effect
    /// </summary>
    private void ChangeText()
    {
        ImpulseSource.GenerateImpulse();
        DOTween.Kill(_scoreText.transform);
        _scoreText.transform.localScale = Vector3.one;
        _scoreText.text = "Score: " + FormatScore(ScoreCounter);
        _scoreText.transform.DOPunchScale(Vector3.one * _punchScale, .2f).OnComplete(() => { _scoreText.transform.localScale = Vector3.one; });
    }

    /// <summary>
    /// Formats the score to a more readable format (e.g., 1k, 100k, 1M, 1B)
    /// </summary>
    /// <param name="score">The score to format</param>
    /// <returns>Formatted score as a string</returns>
    public static string FormatScore(float score)
    {
        if (score >= 1000000000) // 1 Billion
        {
            return (score / 1000000000).ToString("0.##") + "B";
        }
        else if (score >= 1000000) // 1 Million
        {
            return (score / 1000000).ToString("0.##") + "M";
        }
        else if (score >= 1000) // 1 Thousand
        {
            return (score / 1000).ToString("0.##") + "k";
        }
        else
        {
            return score.ToString("0");
        }
    }
}