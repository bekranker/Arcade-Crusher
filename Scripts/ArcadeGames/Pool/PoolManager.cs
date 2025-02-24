using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{

}

[System.Serializable]
public class PoolObject<T> where T : class
{
    public List<IPoolObject<T>> PoolItems = new();
}