using UnityEngine;

public class Spike : Collectables, IMaterial
{
    public Player _player;
    public override void CollectMe()
    {
        _player.Die();
    }

    public void Init(Player player)
    {
        _player = player;
    }
}
