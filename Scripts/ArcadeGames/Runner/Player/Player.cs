using UnityEngine;
using ZilyanusLib.Audio;

public class Player : MonoBehaviour, IDamage
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _defaultHealth;
    private float _healthCounter;
    [SerializeField] private LoseScreen _loseScreen;
    void Start()
    {
        _healthCounter = _defaultHealth;
    }
    public void Die()
    {
        _healthCounter = 0;
        _loseScreen.LoseGame();
        AudioClass.PlayAudio("MiniGames/UFORunner/LOSESOUND");
    }
    public void TakeDamage(float amount)
    {
        if (_healthCounter - amount <= 0)
        {
            Die();
            return;
        }
        _healthCounter -= amount;
    }
}