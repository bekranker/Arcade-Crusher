using UnityEngine;
using System;

public interface IPoolObject
{
    string PoolKey { get; set; }
    void OnInit(PoolManager poolManager);
    void OnReturn();
    void OnGet();
    event Action OnReturnAction, OnGetAction;
}