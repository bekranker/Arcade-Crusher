using UnityEngine;
using UnityEngine.UI;

public class JetPack : MonoBehaviour
{
    [SerializeField] private RunnerMovement _runnerMovement;
    [SerializeField] private Slider _jetpackSlider;
    private float _currentJumpValue;
    [SerializeField] private ParticleSystem _jetPackParticle;

    void Update()
    {
        UpdatePlayerSlider();
    }

    public void ChangeUIVisual()
    {
        _jetpackSlider.maxValue = _currentJumpValue;
    }
    private void UpdatePlayerSlider()
    {
        _jetpackSlider.value = _currentJumpValue;
    }
}