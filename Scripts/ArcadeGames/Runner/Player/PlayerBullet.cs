using ArcadeGames.CrossRoad;
using ArcadeGames.Runner;
using UnityEngine;

public class PlayerBullet : BulletParent<CrossRoad_Enemy>
{
    public override void CollectMe(CrossRoad_Enemy collectable)
    {
        collectable.TakeDamage(999);
        Destroy(gameObject);
    }
}