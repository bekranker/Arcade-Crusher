using System;
using UnityEngine;

public class Hearth : TTMEnvironment
{
    public override string PoolKey { get => "hearth"; set => throw new NotImplementedException(); }

    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;

    public override void CollectMe(MonoBehaviour collectable)
    {
        GeneralHearthManager.Instance.IncreaseHeatlh();
        OnCollect?.Invoke();
        OnDie();
    }

    public override void OnDie()
    {
        OnReturnAction?.Invoke();
        _poolManager.Return(gameObject);
    }

    public override void OnGet()
    {
    }

    public override void OnInit(PoolManager poolManager)
    {
    }

    public override void OnReturn()
    {
    }
}