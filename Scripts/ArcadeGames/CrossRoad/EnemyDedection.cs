using UnityEngine;

namespace ArcadeGames.CrossRoad
{
    [RequireComponent(typeof(CrossRoad_Enemy))]
    public class EnemyDetection : MonoBehaviour
    {
        [SerializeField] private CrossRoad_Enemy _crossRoad_Enemy;
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ICollectable<CrossRoad_Enemy>>(out ICollectable<CrossRoad_Enemy> component))
            {
                component.CollectMe(_crossRoad_Enemy);
            }
        }
    }
}