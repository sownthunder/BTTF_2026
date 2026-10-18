using System.Collections.Generic;
using System.Linq;
using QuanticBrains.Core.Scripts.Agent;
using QuanticBrains.Core.Scripts.Agent.BrainsManager;
using QuanticBrains.Core.Scripts.Agent.Jointed;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations
{
    public class WalkEnvironment : JointedEnvironment, IHotSwappable
    {
        private List<RayPerceptionSensorComponent3D> _feetPerceptionSensors = new List<RayPerceptionSensorComponent3D>();
        public override void Initialize(AgentController ac)
        {
            _feetPerceptionSensors = (ac.GetComponentsInChildren<RayPerceptionSensorComponent3D>(true)).ToList();
        }
        
        public void Enable()
        {
            _feetPerceptionSensors.ForEach(x => x.gameObject.SetActive(true));
        }

        public void Disable()
        {
            _feetPerceptionSensors.ForEach(x => x.gameObject.SetActive(false));
        }
    }
}
