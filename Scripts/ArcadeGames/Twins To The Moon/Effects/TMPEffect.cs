using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
public class TMPEffect : MonoBehaviour, IPoolObject
{
    [SerializeField] private TMP_Text _text;

    public string PoolKey { get => "TextEffect"; set => throw new NotImplementedException(); }

    public event Action OnReturnAction;
    public event Action OnGetAction;
    private PoolManager _poolManager;

    public void InitText(string value, Transform parent)
    {
        _text.color = new Color(Random.Range(0.5f, 1), Random.Range(0.4f, 1), Random.Range(0.6f, 1), 1);
        _text.DOColor(Color.black, 0);
        transform.position = parent.position + Vector3.up;
        transform.SetParent(parent);
        _text.text = "+" + value;
        _text.color = Color.white;
        _text.transform.DOLocalMoveY(_text.transform.localPosition.y + .5f, .3f);
        _text.DOFade(0, 0.3f).OnComplete(() =>
        {
            _poolManager.Return(gameObject);
        });
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
    }
}