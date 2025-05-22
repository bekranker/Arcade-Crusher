using UnityEngine;
public class PlayerDedection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.TryGetComponent(out ICollectable<MonoBehaviour> collectable))
        {
            print("Collectable" + collision.name);
            collectable.CollectMe(this);
        }
    }
}