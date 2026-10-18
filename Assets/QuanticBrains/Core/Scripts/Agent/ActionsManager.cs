using Sirenix.OdinInspector;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [DisableInInlineEditors]
    public abstract class ActionsManager : ScriptableObject
    {
        /// <summary>
        /// Receive Agent Actions
        /// </summary>
        /// <param name="agent">Affected agent</param>
        /// <param name="i">Action starting index (-1)</param>
        /// <param name="continuousActions">List of continuous actions</param>
        /// <param name="discreteActions">List of discrete actions</param>
        public abstract void Receive(AgentController agent, int i, ActionSegment<float> continuousActions, ActionSegment<int> discreteActions);
    }
}
