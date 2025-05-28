using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using ZilyanusLib.Audio;

public class Player : MonoBehaviour, IDamage
{
    public static Player Instance { get; private set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _defaultHealth;
    private float _healthCounter;
    [SerializeField] private LoseScreen _loseScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _healthCounter = _defaultHealth;
    }

    public void Die()
    {
        _healthCounter = 0;
        _loseScreen.LoseGame();
        //AudioClass.PlayAudio("MiniGames/UFORunner/LOSESOUND"); => add this with event Action
    }

    public void TakeDamage(float amount)
    {
        Time.timeScale = 1;
        if (_healthCounter - amount <= 0)
        {
            Die();
            return;
        }
        _healthCounter -= amount;
        //add screen shake here
    }

}