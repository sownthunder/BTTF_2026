using UnityEngine;

namespace QuanticBrains.Core.Scripts.Extensions
{
    public static class Vector3Ex
    {
        
        public static Vector3 NormalizeForObservation(this Vector3 vector)
        {
            vector.x = vector.x.Normalize();
            vector.y = vector.y.Normalize();
            vector.z = vector.z.Normalize();
            return vector;
        }

        public static Vector3 ClampAxis(this Vector3 vector, float min, float max)
        {
            vector.x = Mathf.Clamp(vector.x, min, max);
            vector.y = Mathf.Clamp(vector.y, min, max);
            vector.z = Mathf.Clamp(vector.z, min, max);
            return vector;
        }
    }
}
