using QuanticBrains.Core.Scripts.Agent;
using QuanticBrains.Core.Scripts.Agent.Jointed.Humanoid;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations
{
    public class WalkActions : ActionsManager
    {
        public override void Receive(AgentController agent, int i, ActionSegment<float> continuousActions, ActionSegment<int> discreteActions)
        {
            //52
            var agentHumanoid = (HumanoidAgent)agent;
            agentHumanoid.spine.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.upperChest.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);

            agentHumanoid.leftUpperLeg.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.leftLowerLeg.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.leftFoot.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.rightUpperLeg.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.rightLowerLeg.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.rightFoot.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            
            agentHumanoid.leftUpperArm.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.rightUpperArm.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.leftLowerArm.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.rightLowerArm.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);
            agentHumanoid.head.SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);

            //update joint strength settings
            agentHumanoid.spine.SetJointStrength(continuousActions[++i]);
            agentHumanoid.upperChest.SetJointStrength(continuousActions[++i]);
            agentHumanoid.leftUpperLeg.SetJointStrength(continuousActions[++i]);
            agentHumanoid.leftLowerLeg.SetJointStrength(continuousActions[++i]);
            agentHumanoid.leftFoot.SetJointStrength(continuousActions[++i]);
            agentHumanoid.rightUpperLeg.SetJointStrength(continuousActions[++i]);
            agentHumanoid.rightLowerLeg.SetJointStrength(continuousActions[++i]);
            agentHumanoid.rightFoot.SetJointStrength(continuousActions[++i]);

            agentHumanoid.leftUpperArm.SetJointStrength(continuousActions[++i]);
            agentHumanoid.rightUpperArm.SetJointStrength(continuousActions[++i]);
            agentHumanoid.leftLowerArm.SetJointStrength(continuousActions[++i]);
            agentHumanoid.rightLowerArm.SetJointStrength(continuousActions[++i]);
            agentHumanoid.head.SetJointStrength(continuousActions[++i]);
            
        }
    }
}
