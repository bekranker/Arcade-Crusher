using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGames.CrossRoad
{
    public class CrossRoad_WaveSystem : MonoBehaviour
    {
        [SerializeField] private List<CrossRoad_WaveType> _waves = new();
    }
}