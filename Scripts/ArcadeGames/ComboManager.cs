using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public float comboCount = 0; // The current combo count
    public float comboDuration = 2f; // Duration to reset the combo if no hits are made
    [SerializeField] private float _maximumCombo;
    [SerializeField] private Slider _comboBar;
    public static ComboManager Instance { get; private set; }
    [SerializeField] private ParticleSystem _fireworkParticle;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitComboBar();
    }
    void Update()
    {
        UpdateComboBar();
    }
    void InitComboBar()
    {
        _comboBar.maxValue = _maximumCombo;
        _comboBar.value = comboCount;
    }
    void UpdateComboBar()
    {
        if (comboCount >= 0 && !_isComboActive)
        {
            comboCount -= Time.deltaTime;

        }
        if (_isComboActive)
        {
            if (comboCount >= 0)
            {
                comboCount -= Time.deltaTime;
            }
            if (_comboCounter <= 0)
            {
                _comboCounter = _comboCount;
                _isComboActive = false;
            }
            else
            {
                _comboCounter -= Time.deltaTime;
            }
        }
        _comboBar.value = comboCount;
    }
    private bool _isComboActive = false;
    private float _comboCounter, _comboCount = 1;
    public void Hit(float score)
    {
        comboCount++;
        if (comboCount > _maximumCombo)
        {
            _isComboActive = true;
            if (comboCount <= 8)
                SetParticleSystem(comboCount * 10);
        }
        _comboBar.value = comboCount;
        _comboBar.transform.DOPunchScale(Vector3.one * .15f, .2f).SetUpdate(true);
        _comboBar.transform.DOPunchRotation(Vector3.forward * 15, .2f).SetUpdate(true);
        if (comboCount >= _maximumCombo)
        {
            GeneralScoreHandler.Instance.IncreaseScore(Mathf.RoundToInt(comboCount - (_maximumCombo - 1)) * score);
        }
        else
        {
            GeneralScoreHandler.Instance.IncreaseScore(score);
        }
    }
    [Button("Set Particle System")]
    private void SetParticleSystem(float rateOverTimeValue)
    {
        _fireworkParticle.Play();
        ParticleSystem.EmissionModule emission = _fireworkParticle.emission;
        emission.rateOverTime = rateOverTimeValue;
    }
    private void SetParticleSystemOff()
    {
        ParticleSystem.EmissionModule emission = _fireworkParticle.emission;
        emission.rateOverTime = 0;
        _fireworkParticle.Stop();
    }
}