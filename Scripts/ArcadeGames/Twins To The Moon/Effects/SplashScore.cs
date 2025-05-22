using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections;
public class SplashScore : MonoBehaviour, IPoolObject
{
    [SerializeField] private Image _firstImage, _secondImage;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private ParticleSystem _collectedParticle;
    public string PoolKey { get => "SplashScore"; set => throw new NotImplementedException(); }

    public event Action OnReturnAction;
    public event Action OnGetAction;

    [Button]
    public void InitTMP(string score, Transform parent)
    {
        transform.position = parent.position;
        transform.SetParent(parent);
        _collectedParticle.Play();
        _firstImage.fillAmount = 1;
        _secondImage.fillAmount = 1;
        _text.transform.localPosition = Vector3.zero;
        _text.color = Color.white;
        _text.text = "+" + score;
        transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
        _text.transform.DOLocalMoveY(_text.transform.localPosition.y + 1, .4f);
        _text.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f);
        _firstImage.DOFillAmount(0, .25f).OnComplete(() =>
        {
            _text.color = Color.black;
            _firstImage.fillAmount = 0;
            _text.DOFade(0, .4f);
            _secondImage.DOFillAmount(0, .3f).OnComplete(() =>
            {
                _secondImage.fillAmount = 0;
            });
        });
        StartCoroutine(DelayedReturn());
    }
    private IEnumerator DelayedReturn()
    {
        yield return new WaitForSeconds(0.5f);
        _poolManager.Return(gameObject);
    }
    public void OnGet()
    {
    }

    public void OnInit(PoolManager poolManager)
    {
        _poolManager = poolManager;
    }

    public void OnReturn()
    {
        _poolManager.Return(gameObject);
    }
}