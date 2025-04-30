using UnityEngine;
using System.Collections.Generic;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private List<ParallaxLayer> _parallaxLayers = new();
    public Vector3 ParentDirection;
    [SerializeField] private bool _initialStart;
    void Update()
    {
        if (_initialStart)
        {
            SlideLayers(1);
        }
    }

    public void SlideLayers(float SpeedMultiplier)
    {
        _parallaxLayers?.ForEach((layer) =>
        {
            layer.ChangePosition(ParentDirection * SpeedMultiplier);
        });
    }
}

[System.Serializable]
public class ParallaxLayer
{
    public float Speed; // Katmanın kayma hızı
    public GameObject Layer; // Katmanın GameObject'i
    public Vector3 Direction; // Kayma yönü (1 sağa, -1 sola)

    public void ChangePosition(Vector3 parentDirection)
    {
        if (Layer != null)
        {
            // Layer'ın mevcut pozisyonunu al
            Vector3 currentPosition = Layer.transform.position;

            // Layer'ın yeni pozisyonunu hesapla
            Vector3 newPosition = currentPosition + Vector3.Scale(Direction, parentDirection) * Speed * Time.deltaTime;

            // // Eğer layer ekranın sol sınırını geçerse, sağ taraftan tekrar girmesini sağla
            // if (newPosition.x <= ScreenBounds.LeftCorner().x * -1.5f)
            // {
            //     newPosition.x = ScreenBounds.RightCorner().x * 1.5f;
            // }

            // Layer'ın pozisyonunu güncelle
            Layer.transform.position = newPosition;
        }
    }
}