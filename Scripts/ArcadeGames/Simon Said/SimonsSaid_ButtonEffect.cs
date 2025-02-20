using UnityEngine;

public class SimonsSaid_ButtonEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _buttonParticlePrefab;

    public static SimonsSaid_ButtonEffect Instance { get; set; }

    public void Spawn(Vector3 position)
    {
        Instantiate(_buttonParticlePrefab, position, Quaternion.identity);
    }
}