using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ScoreBooster : TTMEnvironment
{
    public override string PoolKey { get => "scorebooster"; set => throw new NotImplementedException(); }
    [SerializeField] private GameObject _parentSprite;
    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;
    public override void Initialize(TwinsToTheMoonHandler twinsToTheMoonHandler, LoseScreen loseScreen, PoolManager poolManager)
    {
        base.Initialize(twinsToTheMoonHandler, loseScreen, poolManager);
        _parentSprite.SetActive(true);
    }
    public override void CollectMe(MonoBehaviour collectable)
    {
        GeneralScoreHandler.Instance.IncreaseScore(100);
        SplashScore SplashScore = _poolManager.Get("SplashScore").GetComponent<SplashScore>();
        SplashScore.InitTMP("100", transform);
        _parentSprite.SetActive(false);
        StartCoroutine(DelayedReturn());
    }
    private IEnumerator DelayedReturn()
    {
        yield return new WaitForSeconds(0.5f);
        _parentSprite.SetActive(false);
        _poolManager.Return(gameObject);
    }
    public override void OnDie()
    {
    }

    public override void OnGet()
    {
    }

    public override void OnInit(PoolManager poolManager)
    {
        _poolManager = poolManager;
    }

    public override void OnReturn()
    {
    }
}