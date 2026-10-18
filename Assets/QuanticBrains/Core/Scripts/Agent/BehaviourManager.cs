using Sirenix.OdinInspector;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [DisableInInlineEditors]
    public abstract class BehaviourManager : ScriptableObject
    {
        protected AgentController AgentController;
        public virtual void Initialize(AgentController ac)
        {
            AgentController = ac;
        }

        public virtual void ManageTarget(Vector3 currentTarget)
        {
        }
    }
}
