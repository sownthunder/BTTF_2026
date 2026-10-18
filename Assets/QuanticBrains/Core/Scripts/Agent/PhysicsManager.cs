using Sirenix.OdinInspector;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [DisableInInlineEditors]
    public class PhysicsManager : ScriptableObject
    {
        [TabGroup("Physics")]
        public float maxJointSpring = 40000;
        [TabGroup("Physics")]
        public float jointDampen = 5000;
        [TabGroup("Physics")]
        public float maximumForce = 20000;
        [TabGroup("Physics")]
        public float maxAngularVelocity = 50;
    }
}
