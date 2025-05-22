using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TTMEnvironmentLogic
{
    public List<string> Environments;
    public string Parent;
    [Range(0, 100)] public float Possibility;
    [Range(1, 100)] public int MaxSpawnCount;
}
