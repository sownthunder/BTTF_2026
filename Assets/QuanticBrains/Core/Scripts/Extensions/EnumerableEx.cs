using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Extensions
{
    public static class EnumerableEx
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(var item in enumeration)
            {
                action(item);
            }
        }
    }
}
