using Sirenix.OdinInspector;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [DisableInInlineEditors]
    public abstract class Sensor : ScriptableObject
    {
        [ShowInInspector, MultiLineProperty(5)]
        public string description;
        public abstract void Initialize(AgentController agent);
    }
}
