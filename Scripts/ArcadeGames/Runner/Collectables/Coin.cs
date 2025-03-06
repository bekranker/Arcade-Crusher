using UnityEngine;
using ZilyanusLib;
using ZilyanusLib.Audio;
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
        AudioClass.PlayAudio("MiniGames/UFORunner/UFOCOIN", .45f, "General", "Sound", 1, .2f);
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