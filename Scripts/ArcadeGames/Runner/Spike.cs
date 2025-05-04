using System;
using System.Collections;
using UnityEngine;

public class Spike : TTMEnvironment
{
    public override event Action OnCollect;
    private bool _canCollect = true;

    Vector3 _initalStartPosition;

    public override string PoolKey { get => "spike"; set => PoolKey = value; }

    void Update()
    {
        if (transform.position.y < _initalStartPosition.y - 10)
        {
            OnDie();
        }
    }
    public override void CollectMe(MonoBehaviour mono)
    {
        if (!_canCollect) return;
        print("Spike Touched");
        _loseScreen.LoseGame();
        OnCollect?.Invoke();
        StartCoroutine(WaitForCollect());
        _canCollect = false;
    }
    private IEnumerator WaitForCollect()
    {
        yield return new WaitForSeconds(0.5f);
        OnDie();
    }
    public override void OnDie()
    {
        _canCollect = true;
        _poolManager.Return(gameObject);
    }


    public override void OnGet()
    {
        _initalStartPosition = transform.position;
        _canCollect = true;
    }

    public override void OnInit()
    {
    }

    public override void OnReturn()
    {
    }
}
