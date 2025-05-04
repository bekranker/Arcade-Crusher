using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHighGroundSCB", menuName = "ScriptableObjects/HighGroundSCB", order = 1)]
public class HighGroundSCB : ScriptableObject
{
    public float MaxHeight;
    public List<TTMEnvironmentLogic> Environments;
    public List<TTMEnvironmentParallaxLogic> ParallaxObjects;
}
