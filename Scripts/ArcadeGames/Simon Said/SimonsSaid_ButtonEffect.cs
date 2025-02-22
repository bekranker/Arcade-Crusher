using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using ZilyanusLib;
//B
public class SimonsSaid_ButtonEffect : MonoBehaviour
{
    [Header("---Prefabs")]
    [SerializeField] private ParticleSystem _buttonParticlePrefab;
    [SerializeField] private GameObject _decreasingAmountTextPrefab;
    [Header("---Score Text Spawner")]
    [SerializeField] private Transform _pivot;
    [SerializeField] private Transform _parent;
    [SerializeField] private Color _redScoreColor, _greenScoreColor;
    [Header("---Components")]
    [SerializeField] private CinemachineImpulseSource ImpulseSource;
    [SerializeField] private GeneralScoreHandler _scoreHandler;
    [SerializeField] private SimonSaidHealthHandler _simonHealthHandler;
    [Header("---Debug")]
    public bool LoseEverythingOnWorngInput;
    public static SimonsSaid_ButtonEffect Instance { get; set; }
    private string _path = "MiniGames/SimonSaid/";
    void OnEnable()
    {
        SimonSaidHandler.OnListen += CameraShake;
        SimonSaidHandler.OnTrueInput += TrueInputEffect;
        SimonSaidHandler.OnWrongInput += WrongInput;
        GeneralScoreHandler.OnDecrease += SpawnTextRed;
        GeneralScoreHandler.OnIncrease += SpawnTextGreen;
    }
    void OnDisable()
    {
        SimonSaidHandler.OnListen -= CameraShake;
        SimonSaidHandler.OnTrueInput -= TrueInputEffect;
        SimonSaidHandler.OnWrongInput -= WrongInput;
        GeneralScoreHandler.OnDecrease -= SpawnTextRed;
        GeneralScoreHandler.OnIncrease -= SpawnTextGreen;
    }
    public void Spawn(Vector3 position)
    {
        Instantiate(_buttonParticlePrefab, position, Quaternion.identity);
    }
    private void TrueInputEffect(Vector3 position)
    {
        Spawn(position);
        CameraShake(1);
        _scoreHandler.IncreaseScore(100);
    }
    private void WrongInput()
    {
        float decreaseAmount = _scoreHandler.DecreaseScore(_scoreHandler.ScoreCounter);

        if (LoseEverythingOnWorngInput)
        {
            _scoreHandler.DecreaseScore(_scoreHandler.ScoreCounter);
        }
        else
        {
            _scoreHandler.DecreaseScore(_scoreHandler.ScoreCounter / 2);
        }
        CameraShake(1.5f);
        _simonHealthHandler.DecreaseHealth(1);
        ZilyanusLib.Audio.AudioClass.PlayAudio($"{_path}LOSESOUND", .5f);
    }
    /// <summary>
    /// called when Increasing score
    /// </summary>
    private void SpawnTextGreen()
    {
        TMP_Text spawned = SpawnedScoreText();
        spawned.text = "+" + GeneralScoreHandler.FormatScore(100 * _scoreHandler.TrueScoreCounter);
        spawned.color = _greenScoreColor;
    }
    /// <summary>
    /// called when decresing score
    /// </summary>
    private void SpawnTextRed()
    {
        TMP_Text spawned = SpawnedScoreText();
        spawned.text = "-" + GeneralScoreHandler.FormatScore(_scoreHandler.ScoreCounter);
        spawned.color = _redScoreColor;
    }
    private TMP_Text SpawnedScoreText()
    {
        Transform spawnedText = Instantiate(_decreasingAmountTextPrefab, _pivot.position, Quaternion.identity).transform;
        spawnedText.SetParent(_parent);
        spawnedText.transform.localScale = Vector3.one;
        Destroy(spawnedText.gameObject, 1);
        return spawnedText.GetComponentInChildren<TMP_Text>();
    }
    private void CameraShake(float force)
    {
        ImpulseSource.GenerateImpulse(force);
    }
    private void OpenDeadCanvas()
    {

    }
}