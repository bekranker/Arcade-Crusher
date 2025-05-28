using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TTMEnvironmentParallaxLogic
{
    public List<string> Environments = new();
    [SerializeField] public string Parent;
    public List<Vector3> SpawnPoses = new();
}