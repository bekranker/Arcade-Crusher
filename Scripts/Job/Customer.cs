using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private CustomerHandler _costumerHandler;

    public void Init(CustomerHandler costomerHandler)
    {
        _costumerHandler = costomerHandler;
    }
    public void Move(Vector3 targetPoint)
    {
        //Animation.SetBool(Walking);

    }
}