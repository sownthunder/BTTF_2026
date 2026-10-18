using Sirenix.OdinInspector;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [DisableInInlineEditors]
    public abstract class ObservationsManager : ScriptableObject
    {
        public abstract void Collect(AgentController agent, VectorSensor sensor);
    }
}
