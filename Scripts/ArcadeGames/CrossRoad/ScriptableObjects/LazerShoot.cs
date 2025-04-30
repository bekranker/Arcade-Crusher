using UnityEngine;

[CreateAssetMenu(menuName = "EnemyEvents/Lazer Shoot")]
public class LazerShoot : EnemyEventTypeSO
{
    [SerializeField] public EnemyBullet _bulletPrefab;

    public override void ExecuteAction(GameObject @object)
    {
        EnemyBullet spawnedBullet = Instantiate(_bulletPrefab, @object.transform.position, Quaternion.identity);
        spawnedBullet.DirectionToGo.x = Player.Instance.transform.position.x - @object.transform.position.x;
    }
}