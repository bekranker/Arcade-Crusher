using ArcadeGames.CrossRoad;
using ArcadeGames.Runner;
using UnityEngine;

public class EnemyBullet : BulletParent<Player>
{
    public override void CollectMe(Player collectable)
    {
        collectable.TakeDamage(999);
    }
}