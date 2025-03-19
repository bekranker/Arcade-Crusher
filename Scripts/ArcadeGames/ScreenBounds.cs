using UnityEngine;

public static class ScreenBounds
{
    // Ekranın sol sınırını döndürür
    public static Vector2 LeftCorner()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, Camera.main.nearClipPlane));
    }

    // Ekranın sağ sınırını döndürür
    public static Vector2 RightCorner()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.nearClipPlane));
    }
}