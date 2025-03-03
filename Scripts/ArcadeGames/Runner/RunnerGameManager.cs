using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RunnerGameManager : MonoBehaviour
{
    [SerializeReference] public List<LevelData> Levels = new();
    [SerializeField] private RunnerMovement _runnerMovement;
    [SerializeField] private ProcuderalGenerator _procuderalGenerator;
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _levelTMP;
    [SerializeField] private GameObject _nextLevelTexts;

    [HideInInspector] public List<Material> CurrentMaterials = new();
    public Vector2Int LevelIndex;
    [HideInInspector] public ProcuderalRunnerLevelSCB CurrentLevel;
    [HideInInspector] public float CurrentFrequency;
    private bool _startTheGame = false;


    void Start()
    {
        InitializeIndex();
        LevelTextRender();
    }
    void Update()
    {
        if (_startTheGame) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartLevel();
        }
    }
    public void StartLevel()
    {
        _nextLevelTexts.SetActive(false);

        MiniGameController.Instance.ContunieToPlay();
        _startTheGame = true;
    }

    public void NextLevel()
    {
        MiniGameController.Instance.PauseTheGame();
        //opening the UI
        _nextLevelTexts.SetActive(true);
        //sub leveller farklı sayılarda olursa diye
        if (LevelIndex.y + 1 >= Levels[LevelIndex.x].SubLevels.Count)
        {
            LevelIndex.x++;
            LevelIndex.y = 0;
        }
        else
            LevelIndex.y++;

        CurrentLevel = Levels[LevelIndex.x].SubLevels[LevelIndex.y];
        CurrentFrequency = Levels[LevelIndex.x].SubLevels[LevelIndex.y].Frequency;
        CurrentMaterials = Levels[LevelIndex.x].SubLevels[LevelIndex.y].LevelMaterials;

        LevelTextRender();
        Debug.Log("current Level: " + LevelIndex.x + " - " + LevelIndex.y);
        _procuderalGenerator.ReGenerate();
        //we can start next level after giving second
        StartCoroutine(delayForWinning());
    }
    private void InitializeIndex()
    {
        if (Levels[LevelIndex.x].SubLevels == null || Levels[LevelIndex.x].SubLevels.Count <= LevelIndex.y)
        {
            Debug.LogError("Hata: Levels[" + LevelIndex.x + "] içinde geçersiz LevelIndex.y: " + LevelIndex.y);
            return;
        }

        CurrentLevel = Levels[LevelIndex.x].SubLevels[LevelIndex.y];
        CurrentFrequency = CurrentLevel.Frequency;
        CurrentMaterials = CurrentLevel.LevelMaterials;
    }

    private IEnumerator delayForWinning()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        _runnerMovement._loseJustOnes = false;
        _runnerMovement._winJustOnes = false;
        _startTheGame = false;
    }
    public void LevelTextRender()
    {
        _levelTMP.text = LevelIndex.x + " - " + LevelIndex.y.ToString();
    }
}