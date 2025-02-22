using System;
using UnityEngine;

public class MiniGameController : MonoBehaviour, ISingleton<MiniGameController>
{
    public static MiniGameController Instance { get; set; }

    public bool Paused = false;
    public static event Action OnPause, OnContunieToPlay;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PauseTheGame()
    {
        OnPause?.Invoke();
        Paused = true;
    }
    public void ContunieToPlay()
    {
        OnContunieToPlay.Invoke();
        Paused = false;
    }
}