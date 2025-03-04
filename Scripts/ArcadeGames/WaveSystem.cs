using UnityEngine;

public class WaveSystem : WaveSCB
{
    public override void NextWave()
    {
        if (Index.y + 1 > Levels[(int)Index.x].SubLevels.Count)
        {
            Index.y = 0;
            if (Index.x + 1 > Levels.Count)
            {
                print("Index X is at the limit");
                return;
            }
            Index.x++;
        }
    }

    public override void InitWave()
    {
        Index = Vector2.zero;
    }

}