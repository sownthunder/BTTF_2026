using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Helpers
{
    public static class EditorHelpers
    {
        public static IEnumerable GetAllAvatarsForDropdown(Avatar avatar)
        {
            if (avatar != null)
            {
                return avatar.humanDescription.human.Aggregate(
                    new ValueDropdownList<string>(), 
                    (accum, current) =>
                    {
                        accum.Add(current.humanName, current.boneName);
                        return accum;
                    });
            }

            return null;
        }
    }
}
