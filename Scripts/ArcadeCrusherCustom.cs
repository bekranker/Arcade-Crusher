using UnityEngine;

namespace ArcadeCrusher
{
    public static class ArcadeCrusherCustom
    {
        public static class ArcadeCrusherMath
        {
            public static float Sign(float f)
            {
                if (f == 0)
                {
                    return 0;
                }
                return (f > 0f) ? 1f : (-1f);
            }
        }
        public static bool OffTheScreen(Transform objectT, Camera cam, Vector3 extraStep = new Vector3())
        {
            Vector3 rightCorner = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) + extraStep;
            return objectT.position.x > rightCorner.x || objectT.position.x < -rightCorner.x ||
                   objectT.position.y > rightCorner.y || objectT.position.y < -rightCorner.y;
        }
    }
}