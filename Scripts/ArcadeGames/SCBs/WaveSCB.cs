using System.Collections.Generic;
using UnityEngine;

public abstract class WaveSCB : MonoBehaviour
{
    public Vector2 Index;
    public ProcuderalRunnerLevelSCB CurrentLevelData;
    public List<LevelData> Levels = new();
    public abstract void NextWave();
    public abstract void InitWave();

}