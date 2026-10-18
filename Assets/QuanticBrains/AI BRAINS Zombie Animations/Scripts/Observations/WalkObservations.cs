using QuanticBrains.Core.Scripts.Agent;
using QuanticBrains.Core.Scripts.Agent.Jointed.BodyParts;
using QuanticBrains.Core.Scripts.Agent.Jointed.Humanoid;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations
{
    public class WalkObservations : ObservationsManager
    {
        public LayerMask groundLayer;
        public override void Collect(AgentController agent, VectorSensor sensor)
        {
            var agentHumanoid = (HumanoidAgent)agent;

            var cubeForward = agentHumanoid.orientationHelper.transform.forward;

            //velocity we want to match
            var velGoal = cubeForward * agentHumanoid.agentDefinition.TargetWalkingSpeed;
            //ragdoll's avg vel
            var avgVel = agentHumanoid.GetAvgVelocity();

            sensor.AddObservation(agentHumanoid.IsReady);

            //current ragdoll velocity. normalized
            sensor.AddObservation(Vector3.Distance(velGoal, avgVel));
            //avg body vel relative to cube
            sensor.AddObservation(agentHumanoid.orientationHelper.transform.InverseTransformDirection(avgVel));
            //vel goal relative to cube
            sensor.AddObservation(agentHumanoid.orientationHelper.transform.InverseTransformDirection(velGoal));

            //rotation deltas
            sensor.AddObservation(Quaternion.FromToRotation(agentHumanoid.hips.transform.forward, cubeForward));
            sensor.AddObservation(Quaternion.FromToRotation(agentHumanoid.head.transform.forward, cubeForward));

            //Position of target position relative to cube
            sensor.AddObservation(agentHumanoid.orientationHelper.transform.InverseTransformPoint(agentHumanoid.target.transform.position));

            sensor.AddObservation(agentHumanoid.leftFoot.canCollide);
            sensor.AddObservation(agentHumanoid.rightFoot.canCollide);

            agentHumanoid.bodyParts
                .ForEach(bodyPart => CollectObservationBodyPart(agentHumanoid, bodyPart, sensor));
        }
        
        private void CollectObservationBodyPart(HumanoidAgent humanoidAgent, BodyPartBase bp, VectorSensor sensor)
        {
            if (bp == humanoidAgent.leftFootToe || bp == humanoidAgent.rightFootToe)
            {
                return;
            }
            
            //GROUND CHECK
            sensor.AddObservation(bp.IsTouchingWith(groundLayer)); // Is this bp touching the ground

            //Get position relative to hips in the context of our orientation cube's space
            sensor.AddObservation(
                humanoidAgent.orientationHelper.transform.InverseTransformDirection(bp.transform.position - humanoidAgent.hips.transform.position));

            //Get velocities in the context of our orientation cube's space
            //Note: You can get these velocities in world space as well but it may not train as well.
            sensor.AddObservation(humanoidAgent.orientationHelper.transform.InverseTransformDirection(bp.GetVelocity()));
            sensor.AddObservation(humanoidAgent.orientationHelper.transform.InverseTransformDirection(bp.GetAngularVelocity()));

            if (bp == humanoidAgent.hips || bp == humanoidAgent.leftHand || bp == humanoidAgent.rightHand)
            {
                return;
            }
            
            sensor.AddObservation(bp.transform.localRotation);
            sensor.AddObservation(bp.currentStrength / humanoidAgent.physicsManager.maximumForce);
        }
    }
}
