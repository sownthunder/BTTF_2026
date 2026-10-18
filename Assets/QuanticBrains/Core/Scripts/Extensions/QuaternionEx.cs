using UnityEngine;

namespace QuanticBrains.Core.Scripts.Extensions
{
    public static class QuaternionEx
    {
        public static Quaternion NormalizeForObservation(this Quaternion quaternion)
        {
            quaternion.x = quaternion.x.Normalize();
            quaternion.y = quaternion.y.Normalize();
            quaternion.z = quaternion.z.Normalize();
            quaternion.w = quaternion.w.Normalize();
            return quaternion;
        }
    }
}
