using UnityEngine;
public class PlayerDedection : MonoBehaviour
{
    [SerializeField] private Player _player;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.TryGetComponent(out ICollectable<Player> collectable))
        {
            collectable.CollectMe(_player);
        }
    }
}