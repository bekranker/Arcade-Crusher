using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeHandler : MonoBehaviour
{
    [SerializeField] private List<Image> _chargePills = new();

    private void Start()
    {
        ChargeHandler charges = GameObject.FindAnyObjectByType<ChargeHandler>();
        if (charges != null)
        {
            if (charges != this)
            {
                Destroy(charges);
            }
        }
    }
}