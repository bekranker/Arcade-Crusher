using DG.Tweening;
using UnityEngine;

public class PlayerMovementEffect : MonoBehaviour
{
    [SerializeField] private BaseMovement _baseMovement;
    [SerializeField] private SpriteRenderer _sp;
    [SerializeField] private TwinsToTheMoonHandler _twinsToTheMoonHandler;
    [SerializeField] private ParticleSystem _jumpParticle;
    private bool _flip;
    void OnEnable()
    {
        _twinsToTheMoonHandler.OnJump += OnJump;
    }
    void OnDisable()
    {
        _twinsToTheMoonHandler.OnJump -= OnJump;
    }

    void Update()
    {
        SetAngle();
    }
    private void SetAngle()
    {
        if (_flip) return;

        if (_baseMovement.MovementInput.x == 0)
        {
            return;
        }
        float targetAngle = _baseMovement.MovementInput.x * -3f; // Adjust multiplier as needed
        _sp.transform.DORotate(new Vector3(0, 0, targetAngle), 0.1f); // Adjust duration as needed
    }
    private void OnJump()
    {
        _flip = true;
        DOTween.Kill(_sp.transform);
        _sp.transform.localScale = new Vector3(1, 1, 1);
        _sp.transform.DOPunchScale(Vector3.one * .3f, 0.2f); // Adjust duration and strength as needed
        _sp.transform.rotation = Quaternion.Euler(0, 0, 0);
        _sp.transform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360).OnComplete(() => _flip = false); // Adjust duration as needed
    }
}