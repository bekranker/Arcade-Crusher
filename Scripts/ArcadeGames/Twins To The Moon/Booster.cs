using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
public class Booster : TTMEnvironment
{
    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;
    [SerializeField] private ParticleSystem _jumpParticle;
    [SerializeField] private Animator _trampolineAnimator;
    [SerializeField] private SpriteRenderer _sp;
    private bool _canCollect = true;

    public override string PoolKey { get => "booster"; set => PoolKey = value; }

    public void Init(TwinsToTheMoonHandler twinsToTheMoonHandler)
    {
        _twinsToTheMoonHandler = twinsToTheMoonHandler;
    }
    public override void CollectMe(MonoBehaviour collectable)
    {
        if (!_canCollect) return;
        DOTween.Kill(_sp.transform);
        _sp.transform.DOPunchScale(Vector3.one, 0.2f);
        _jumpParticle.Play();
        _trampolineAnimator.SetTrigger("Jump");
        _poolManager.Get("TextEffect").GetComponent<TMPEffect>().InitText("25", transform);
        _twinsToTheMoonHandler.Jump = true;
        _twinsToTheMoonHandler.PushForce(_twinsToTheMoonHandler.JumpForce);
        _canCollect = false;
        ComboManager.Instance.Hit(25);
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

    public override void OnInit(PoolManager poolManager)
    {
        _poolManager = poolManager;
    }

    public override void OnReturn()
    {
    }

    public override void OnGet()
    {
    }
}