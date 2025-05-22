using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MovePlatform : TTMEnvironment
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _delay;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private Transform _platform;
    public override event System.Action OnCollect;
    public override event System.Action OnReturnAction;
    public override event System.Action OnGetAction;

    public override string PoolKey { get => "movespike"; set => PoolKey = value; }

    private void MoveThePlatform()
    {
        _platform.DOLocalMove(_endPosition.localPosition, _speed).OnComplete(() =>
        {
            DOVirtual.DelayedCall(_delay, () =>
            {
                _platform.DOLocalMove(_startPosition.localPosition, _speed).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(_delay, () => MoveThePlatform());
                }).SetEase(Ease.Linear);
            });
        }).SetEase(Ease.Linear);
    }
    Vector3 _initalStartPosition;
    void Update()
    {
        if (transform.position.y < _initalStartPosition.y - 9)
        {
            OnDie();
        }
    }
    public override void OnDie()
    {
        DOTween.Kill(_platform);
        _poolManager.Return(gameObject);
    }
    private bool _canCollect = true;
    public override void CollectMe(MonoBehaviour collectable)
    {
        if (!_canCollect) return;
        GeneralHearthManager.Instance.DecreaseHealth();
        OnCollect?.Invoke();
        StartCoroutine(WaitForCollect());
        _canCollect = false;
    }
    private IEnumerator WaitForCollect()
    {
        yield return new WaitForSeconds(0.5f);
        OnDie();
    }
    public override void OnInit(PoolManager poolManager)
    {
        _poolManager = poolManager;
        _platform.localPosition = Random.Range(0, 2) == 0 ? _startPosition.localPosition : _endPosition.localPosition;

        _canCollect = true;
        _initalStartPosition = transform.position;
        MoveThePlatform();
    }
    public override void OnReturn()
    {
        DOTween.Kill(_platform);
    }
    public override void OnGet()
    {
        _platform.localPosition = Random.Range(0, 2) == 0 ? _startPosition.localPosition : _endPosition.localPosition;

        _canCollect = true;
        _initalStartPosition = transform.position;
        MoveThePlatform();
    }
}