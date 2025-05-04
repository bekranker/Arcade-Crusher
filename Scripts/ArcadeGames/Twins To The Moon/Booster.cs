using System;
using System.Collections;
using UnityEngine;

public class Booster : TTMEnvironment
{
    public override event Action OnCollect;
    [SerializeField] private Animator _trampolineAnimator;
    private bool _canCollect = true;

    public override string PoolKey { get => "booster"; set => PoolKey = value; }

    public void Init(TwinsToTheMoonHandler twinsToTheMoonHandler)
    {
        _twinsToTheMoonHandler = twinsToTheMoonHandler;
    }
    public override void CollectMe(MonoBehaviour collectable)
    {
        if (!_canCollect) return;
        _trampolineAnimator.SetTrigger("Jump");
        _twinsToTheMoonHandler.Jump = true;
        _twinsToTheMoonHandler.PushForce(_twinsToTheMoonHandler.JumpForce);
        _canCollect = false;
        GeneralScoreHandler.Instance.IncreaseScore(25);
        StartCoroutine(WaitForAnimation());
    }
    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        OnDie();
    }
    public override void OnDie()
    {
        _canCollect = true;
        _poolManager.Return(gameObject);
    }

    public override void OnInit()
    {

    }

    public override void OnReturn()
    {
    }

    public override void OnGet()
    {
    }
}