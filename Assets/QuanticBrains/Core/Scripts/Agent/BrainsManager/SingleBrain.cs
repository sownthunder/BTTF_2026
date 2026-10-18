using System.Collections.Generic;

namespace QuanticBrains.Core.Scripts.Agent.BrainsManager
{
    public class SingleBrain : BrainsManager
    {
        public AgentDefinition definition;
        protected override void InitBrains()
        {
            Definitions = new List<AgentDefinition> { definition };
            base.InitBrains();
        }
        protected override void Initialize(AgentController ac)
        {
        }
    }
}
