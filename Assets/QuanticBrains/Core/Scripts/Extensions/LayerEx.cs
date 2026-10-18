using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Extensions
{
    public static class LayerEx
    {
        private const int MAXLayers = sizeof(int) * 8; //Unity only allows 32 layers (32 bits)
        
        
        public static void Add(this ref LayerMask mask, int layer)
        {
            mask |= (1 << layer);
        }
        
        public static void Remove(this ref LayerMask mask, int layer)
        {
            mask &= ~(1 << layer);
        }
        
        public static bool ContainsAny(this LayerMask mask, LayerMask otherMask)
        {
            for (var i = 0; i < MAXLayers; i++)
            {
                if (mask == (mask | (1 << i)) && otherMask == (otherMask | (1 << i)))
                {
                    return true;
                }
            }

            return false;
        }
        
    }
}
