using UnityEngine;

public class Spike : Collectables, IMaterial
{
    private Player _player;
    public override void CollectMe()
    {
    }

    public void Init(Player player)
    {
        _player.Die();
    }
}
