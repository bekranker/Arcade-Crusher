using UnityEngine;
public class PlayerDedection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Collectables collectable))
        {
            collectable.CollectMe();
        }
    }
}