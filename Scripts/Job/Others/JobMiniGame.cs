using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class JobMiniGame : MonoBehaviour
{
    public List<ARROW_KEYS> _keys = new();

    private void Init()
    {
        ARROW_KEYS[] values = (ARROW_KEYS[])Enum.GetValues(typeof(ARROW_KEYS));
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, values.Length);
            _keys.Add(values[index]);
        }
    }
}