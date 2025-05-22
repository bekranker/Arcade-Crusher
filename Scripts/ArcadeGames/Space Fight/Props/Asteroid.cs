using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using ArcadeCrusher;
using System.Collections;
public class Asteroid : SpaceFightEnvironment, IDamageProp
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sp;
    [SerializeField] private Sprite _damageSprite, _normalSprite;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float _maximumHealth;
    [SerializeField] private Player _player;
    private float _healthCounter;
    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;
    public string PoolName;
    public override string PoolKey
    { get => PoolName; set => throw new NotImplementedException(); }

    private void Update()
    {
        RotateAsteroid();
        MoveAsteroid();
        if (ArcadeCrusherCustom.OffTheScreen(transform, Camera.main, Vector3.one * 2))
        {
            Vector3 direction = _player.transform.position - transform.position;
            transform.up = direction;
        }
    }

    private void RotateAsteroid()
    {
        _sp.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void MoveAsteroid()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    public override void CollectMe(MonoBehaviour collectable)
    {
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
    private bool _isDamaged = false;
    public void ApplyDamage(float amount)
    {
        if (!_isDamaged)
            StartCoroutine(DamageEffectIE());
        _healthCounter -= amount;
        if (_healthCounter <= 0)
        {
            _poolManager.Return(gameObject);
            OnCollect?.Invoke();
        }
    }
    private IEnumerator DamageEffectIE()
    {
        _sp.sprite = _damageSprite;
        yield return new WaitForSeconds(0.1f);
        _sp.sprite = _normalSprite;
        _isDamaged = false;
    }
    public override void InitSpaceFightEnvironment(PoolManager poolManager, Transform parent, Player player = null)
    {
        _poolManager = poolManager;
        Vector3 direction = player.transform.position - transform.position;
        transform.up = direction;
        _sp.color = new Color(1, 1, 1, 0);
        _collider.enabled = false;
        _sp.DOFade(1, 0.5f).OnComplete(() =>
        {
            _collider.enabled = true;
        });
        transform.position = player.transform.position + new Vector3(-direction.normalized.x * 5, Random.Range(-5, 5), 0) * 5;
        _player = player;
        _healthCounter = _maximumHealth;
    }
}