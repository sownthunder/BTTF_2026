using System.Collections.Generic;
using System.Linq;
using QuanticBrains.Core.Scripts.Extensions;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.BrainsManager
{
    public abstract class BrainsManager : MonoBehaviour
    {
        protected List<AgentDefinition> Definitions;
        protected List<AgentDefinition> DefinitionsInstantiated;
        protected AgentController Agent;
        private AgentDefinition _currentDefinition;
        
        
        protected virtual void InitBrains()
        {
            Agent = GetComponent<AgentController>();
            if (DefinitionsInstantiated != null)
            {
                return;
            }
            
            Agent.PrepareAgent();
            
            DefinitionsInstantiated = Definitions
                .Select(definition => Instantiate(definition)
                    .Also(s=> s.Initialize(Agent))
                ).ToList();
            
            Initialize(Agent);
            _currentDefinition    = DefinitionsInstantiated[0];
            Agent.agentDefinition = DefinitionsInstantiated[0];
            Agent.agentDefinition.Enable();
        }

        protected abstract void Initialize(AgentController ac);
        
        protected void ChangeDefinition(AgentDefinition next)
        {
            if (_currentDefinition == next)
            {
                return;
            }
            
            if (_currentDefinition != null)
            {
                _currentDefinition.Disable();
            }
            
            Agent.agentDefinition = next;
            _currentDefinition = next;
            next.Enable();
        }

        protected void DisableAllBrains()
        {
            DefinitionsInstantiated.ForEach(x=> x.Disable());
            Agent.agentDefinition = null;
            _currentDefinition = null;
            Definitions.Clear();
            DefinitionsInstantiated = null;
        }
    }
}
