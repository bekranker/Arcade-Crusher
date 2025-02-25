using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Level", menuName = "Procuderal Runner / Create Level", order = 1)]
public class ProcuderalRunnerLevelSCB : ScriptableObject
{
    public List<BiomSCB> Biom = new();
    public int Length;
}