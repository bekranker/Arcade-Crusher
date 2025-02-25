using System;
using UnityEngine;

public abstract class Collectables : MonoBehaviour, ICollectable
{
    public abstract void CollectMe();
}