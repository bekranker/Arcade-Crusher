using Unity.Cinemachine;
using UnityEngine;

public class SimonsSaid_ButtonEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _buttonParticlePrefab;
    [SerializeField] private CinemachineImpulseSource ImpulseSource;
    public static SimonsSaid_ButtonEffect Instance { get; set; }

    void OnEnable()
    {
        SimonSaidHandler.OnListen += CameraShake;
        SimonSaidHandler.OnTrueInput += TrueInputEffect;
    }
    void OnDisable()
    {
        SimonSaidHandler.OnListen -= CameraShake;
        SimonSaidHandler.OnTrueInput -= TrueInputEffect;

    }
    public void Spawn(Vector3 position)
    {
        Instantiate(_buttonParticlePrefab, position, Quaternion.identity);
    }
    private void TrueInputEffect(Vector3 position)
    {
        Spawn(position);
        CameraShake();
    }
    private void WrongInput()
    {

    }
    private void CameraShake()
    {
        ImpulseSource.GenerateImpulse();

    }
}