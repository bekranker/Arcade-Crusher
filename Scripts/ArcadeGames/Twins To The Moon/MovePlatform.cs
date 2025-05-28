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
    [SerializeField] private Transform _obstacleSpriteParent;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public override event System.Action OnCollect;
    public override event System.Action OnReturnAction;
    public override event System.Action OnGetAction;

    public override string PoolKey { get => "movespike"; set => PoolKey = value; }

    public override void Initialize(TwinsToTheMoonHandler twinsToTheMoonHandler, LoseScreen loseScreen, PoolManager poolManager)
    {
        base.Initialize(twinsToTheMoonHandler, loseScreen, poolManager);
        _spriteRenderer.color = new Color(1, 1, 1, 1);
        DOTween.Kill(_spriteRenderer);
        DOTween.Kill(_platform);
        int random = Random.Range(0, 2);
        _platform.localPosition = random == 0 ? _startPosition.localPosition : _endPosition.localPosition;
        _obstacleSpriteParent.localScale = random == 0 ? new Vector2(-1, 1) : new Vector2(1, 1);
        _canCollect = true;
        _initalStartPosition = transform.position;
        MoveThePlatform();
    }
    private void MoveThePlatform()
    {
        _obstacleSpriteParent.localScale = _obstacleSpriteParent.localScale.x < 0 ? new Vector2(1, 1) : new Vector2(-1, 1);
        _platform.DOLocalMove(_endPosition.localPosition, _speed).OnComplete(() =>
        {
            DOVirtual.DelayedCall(_delay, () =>
            {
                _obstacleSpriteParent.localScale = _obstacleSpriteParent.localScale.x < 0 ? new Vector2(1, 1) : new Vector2(-1, 1);
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
        if (transform.position.y < _initalStartPosition.y - 15)
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
        DOTween.Kill(_platform);
        GeneralHearthManager.Instance.DecreaseHealth();
        OnCollect?.Invoke();
        _canCollect = false;
        _platform.DOLocalMoveY(-3, .5f).SetEase(Ease.InBack);
        _platform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            _platform.localRotation = Quaternion.Euler(0, 0, 0);
            _spriteRenderer.DOFade(0, 0.5f);
            StartCoroutine(WaitForCollect());
        });
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
    }
    public override void OnReturn()
    {
        DOTween.Kill(_platform);
    }
    public override void OnGet()
    {

    }
}