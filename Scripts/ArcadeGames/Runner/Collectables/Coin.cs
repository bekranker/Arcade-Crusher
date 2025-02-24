using UnityEngine;

public class Coin : MonoBehaviour, ICollectable, IPoolObject<Coin>
{
    public void CollectMe()
    {
    }
}