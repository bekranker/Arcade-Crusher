using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private List<ParallaxLayer> _parallaxLayers = new();

    void Update()
    {
        SlideLayers();
    }
    void SlideLayers()
    {
        _parallaxLayers?.ForEach((layer) =>
        {
            if (layer.Layer != null)
            {
                layer.Layer.transform.localPosition += layer.Direction * Vector3.right * layer.Speed * Time.deltaTime;
            }
        });
    }
}

[Serializable]
public class ParallaxLayer
{
    public float Speed;
    public GameObject Layer;
    public int Direction;
}