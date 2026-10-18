using QuanticBrains.Core.Scripts.Agent;
using QuanticBrains.Core.Scripts.Agent.Jointed.Humanoid;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations
{
    public class StandupActions : ActionsManager
    {
        public override void Receive(AgentController agent, int i, ActionSegment<float> continuousActions, ActionSegment<int> discreteActions)
        {
            if (!((HumanoidAgent) agent).IsReady)
                return;
            
            var ha = (HumanoidAgent)agent;
            ha.spine.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.upperChest.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);

            ha.leftUpperLeg.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.leftLowerLeg.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.leftFoot.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.rightUpperLeg.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.rightLowerLeg.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.rightFoot.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);

            ha.leftUpperArm.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.rightUpperArm.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.leftLowerArm.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.rightLowerArm.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.leftHand.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.rightHand.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            ha.head.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
        }
    }
}


