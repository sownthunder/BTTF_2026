using Sirenix.OdinInspector;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [DisableInInlineEditors]
    public abstract class HeuristicManager : ScriptableObject
    {
        public abstract void Manage(AgentController agent, in ActionBuffers actionsOut);
    }
}
