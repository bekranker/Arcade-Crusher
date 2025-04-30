using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHighGroundSCB", menuName = "ScriptableObjects/HighGroundSCB", order = 1)]
public class HighGroundSCB : ScriptableObject
{
    public float Height;
    public List<Layer> Environments;
}
public class Layer
{
    public GameObject LayerPrefab;
    public Transform ParentLayer;
}