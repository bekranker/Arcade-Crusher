using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fireworkParticle;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private Image _bgImage;
    [SerializeField] private GeneralScoreHandler _generalScoreHandler;
    void OnEnable()
    {
        _generalScoreHandler.OnIncreaseStart += CreateEffect;
    }
    void OnDisable()
    {
        _generalScoreHandler.OnIncreaseStart -= CreateEffect;
    }
    /// <summary>
    /// Score background Image effect
    /// </summary>
    public void CreateEffect()
    {
        _fireworkParticle.Play();
        DOTween.Kill(_bgImage);
        _bgImage.color = new Color(1, 1, 1, .5f);
        _bgImage.DOFade(0, 1f);
    }
}