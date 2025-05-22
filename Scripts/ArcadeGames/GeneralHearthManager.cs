using UnityEngine;

public class GeneralHearthManager : MonoBehaviour
{
    [SerializeField] private int _hearthCount = 3;
    [SerializeField] private GameObject[] _hearths;
    [SerializeField] private LoseScreen _loseScreen;

    public static GeneralHearthManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    public void DecreaseHealth()
    {
        _hearthCount--;
        if (_hearthCount <= 0)
        {
            _loseScreen.LoseGame();
        }
        _hearths[_hearthCount].SetActive(false);
    }
    public void IncreaseHeatlh()
    {
        if (_hearthCount < _hearths.Length)
        {
            _hearths[_hearthCount].SetActive(true);
            _hearthCount++;
        }
    }
}