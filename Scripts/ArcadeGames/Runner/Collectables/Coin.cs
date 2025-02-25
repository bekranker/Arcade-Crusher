using UnityEngine;

public class Coin : Collectables, IPoolObject
{
    public string PoolKey { get => "Coin"; set => value = default; }

    public override void CollectMe()
    {
        print("sa");
    }

    public Coin GetItem()
    {
        return this;
    }

    public void SetItem(Coin item)
    {
        Debug.Log("Coin set in pool");
    }
}