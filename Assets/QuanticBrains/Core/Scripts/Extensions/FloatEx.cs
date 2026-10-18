namespace QuanticBrains.Core.Scripts.Extensions
{
    public static class Float3Ex
    {
        public static float Normalize(this float number)
        {
            return (number + 1) * 0.5f;
        }
    }
}
