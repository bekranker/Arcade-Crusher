using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private GameObject _loseScreen;
    public static event Action OnLose;
    [SerializeField] private CinemachineImpulseSource _screenShake;
    private bool _lose;

    void Update()
    {
        if (!_lose) return;
        if (MiniGameController.Instance.Paused)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return;
            }
        }
    }
    public void LoseGame()
    {
        if (_lose) return;
        OnLose?.Invoke();
        _loseScreen.SetActive(true);
        MiniGameController.Instance.PauseTheGame();
        _screenShake.GenerateImpulse();
        _lose = true;
    }
}
