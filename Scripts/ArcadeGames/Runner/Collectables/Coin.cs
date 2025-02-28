using UnityEngine;

public class Coin : Collectables, IPoolObject, IMaterial
{
    public string PoolKey { get => "Coin"; set => value = default; }
    GeneralScoreHandler _generalScoreHandler;
    Player _player;
    void Start()
    {
        _generalScoreHandler = FindAnyObjectByType<GeneralScoreHandler>();
    }
    public override void CollectMe()
    {
        _generalScoreHandler.IncreaseScore(100);
        gameObject.SetActive(false);
    }

    public Coin GetItem()
    {
        return this;
    }

    public void SetItem(Coin item)
    {
        Debug.Log("Coin set in pool");
    }

    public void Init(Player player)
    {
        _player = player;
    }
}