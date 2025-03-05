using UnityEngine;
public class PlayerDedection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.attachedRigidbody.TryGetComponent(out Collectables collectable))
        {
            collectable.CollectMe();
        }
    }
}