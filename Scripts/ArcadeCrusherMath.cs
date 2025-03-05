using UnityEngine;

namespace ArcadeCrusher
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
}