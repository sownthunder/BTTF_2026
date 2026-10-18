using System;

namespace QuanticBrains.Core.Scripts.Extensions
{
    public static class ObjectEx
    {
        // Kotlin: fun <T> T.also(block: (T) -> Unit): T
        public static T Also<T>(this T self, Action<T> block)
        {
            block(self);
            return self;
        }
    }
}
