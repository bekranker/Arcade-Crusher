using UnityEngine;

public class Vault : MonoBehaviour, IObjectInteractable
{
    [SerializeField] private GameObject _miniGameScreen;
    [SerializeField] private JobMiniGame _jobMiniGame;

    void Awake()
    {
        _miniGameScreen.SetActive(false);
    }
    public void ExecuteInteraction()
    {
        TimeHandler.Instance.Freeze();
        _miniGameScreen.SetActive(true);
        _jobMiniGame.Init();
    }
}
