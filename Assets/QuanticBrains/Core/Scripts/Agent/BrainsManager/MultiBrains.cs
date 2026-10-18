using System.Collections.Generic;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.BrainsManager
{
    public abstract class MultiBrains : BrainsManager
    {
        [HideInInspector]
        public List<AgentDefinition> definitions;
        protected override void InitBrains()
        {
            Definitions = definitions;
            base.InitBrains();
        }

        protected abstract override void Initialize(AgentController ac);
    }
}
