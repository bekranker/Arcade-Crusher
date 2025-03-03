using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Level", menuName = "Procuderal Runner / Create Level", order = 1)]
public class ProcuderalRunnerLevelSCB : ScriptableObject
{
    public List<Material> LevelMaterials = new();
    public float Frequency;
    public int Length;

    public int Index { get; set; }

}
[System.Serializable]
public class LevelData
{
    public List<ProcuderalRunnerLevelSCB> SubLevels = new();
}