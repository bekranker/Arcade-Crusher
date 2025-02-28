using UnityEngine;

public static class ScreenBoundsChecker
{
    public static bool IsTouchingScreenBounds(GameObject obj)
    {
        if (Camera.main == null) return false;

        Camera cam = Camera.main;

        // Ekranın sol alt ve sağ üst köşelerinin dünya koordinatları
        Vector3 minScreenBounds = cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 maxScreenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        // Nesnenin pozisyonunu al
        Vector3 objPos = obj.transform.position;

        // Nesnenin ekran sınırlarına değip değmediğini kontrol et
        bool touching = objPos.x <= minScreenBounds.x || objPos.x >= maxScreenBounds.x ||
                        objPos.y <= minScreenBounds.y || objPos.y >= maxScreenBounds.y;

        return touching;
    }
}
