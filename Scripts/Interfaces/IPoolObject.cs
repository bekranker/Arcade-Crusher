using UnityEngine;

public interface IPoolObject
{
    string PoolKey { get; set; }
    void OnInit();
    void OnReturn();
    void OnGet();
}