using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ZilyanusLib.Audio;

public class JetPack : MonoBehaviour
{
    [Header("---Components")]
    [SerializeField] private RunnerMovement _runnerMovement;
    [SerializeField] private AudioSource _jetPackSound;
    [SerializeField] private Grounded _myGorunded;
    [Header("---UI")]
    [SerializeField] private Slider _jetpackSlider;
    [Header("---Particles")]
    [SerializeField] private ParticleSystem _jetPackParticle;
    void Start()
    {
        ChangeUIVisual();
    }
    void Update()
    {
        UpdatePlayerSlider();
    }
    void OnEnable()
    {
        _runnerMovement.OnJumpStart += OpenParticle;
        _runnerMovement.OnJumpEnd += CloseParticle;
    }
    void OnDisable()
    {
        _runnerMovement.OnJumpStart -= OpenParticle;
        _runnerMovement.OnJumpEnd -= CloseParticle;
    }
    private void OpenParticle()
    {
        _jetPackParticle.Play();
        _jetPackSound.Play();
        if (_myGorunded.IsGrounded())
        {
            AudioClass.PlayAudio("MiniGames/UFORunner/UFOJUMP", 1, "General", "Sound", 1, .2f);
        }
    }
    private void CloseParticle()
    {
        _jetPackParticle.Stop();
        _jetPackSound.Stop();
    }
    public void ChangeUIVisual()
    {
        _jetpackSlider.maxValue = _runnerMovement.CurrentJumpValue;
    }
    private void UpdatePlayerSlider()
    {
        _jetpackSlider.value = _runnerMovement.CurrentJumpValue;
    }

}