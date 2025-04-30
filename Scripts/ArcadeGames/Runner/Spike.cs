using System;
using UnityEngine;

public class Spike : Collectables, IMaterial
{
    public Player _player;

    public override event Action OnCollect;

    public override void CollectMe(MonoBehaviour mono)
    {
        _player.Die();
        OnCollect?.Invoke();
    }

    public void Init(Player player)
    {
        _player = player;
    }
}
