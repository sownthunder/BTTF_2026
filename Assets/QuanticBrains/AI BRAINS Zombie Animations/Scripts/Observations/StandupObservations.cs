using QuanticBrains.Core.Scripts.Agent;
using QuanticBrains.Core.Scripts.Agent.Jointed.BodyParts;
using QuanticBrains.Core.Scripts.Agent.Jointed.Humanoid;
using QuanticBrains.Core.Scripts.Extensions;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations
{
    public class StandupObservations : ObservationsManager
    {
        public LayerMask groundLayer;
        public float desiredHeight = 1.75f;
        private HumanoidAgent _humanoidAgent;
        public override void Collect(AgentController agent, VectorSensor sensor)
        {
            _humanoidAgent = (HumanoidAgent)agent;
            sensor.AddObservation(Mathf.Clamp(GetHeadHeight() / desiredHeight, 0.0f, 1.0f));
            _humanoidAgent.bodyParts.ForEach(bodyPart => CollectObservationBodyPart(_humanoidAgent, bodyPart, sensor));
        }

        private void CollectObservationBodyPart(HumanoidAgent humanoidAgent, BodyPartBase bp, VectorSensor sensor)
        {
            //GROUND CHECK
            sensor.AddObservation(bp.IsTouchingWith(groundLayer)); // Is this bp touching the ground

            //Get position relative to hips in the context of our orientation cube's space
            sensor.AddObservation(humanoidAgent.transform.InverseTransformDirection(bp.transform.position - humanoidAgent.hips.transform.position).NormalizeForObservation());

            //Get velocities in the context of our orientation cube's space
            //Note: You can get these velocities in world space as well but it may not train as well.   //ToDo: Cambiar 10.0f por agentdefinition etc
            sensor.AddObservation(humanoidAgent.transform.InverseTransformDirection((bp.GetVelocity() / 10.0f).ClampAxis(0, 1)));
            sensor.AddObservation(
                humanoidAgent.transform.InverseTransformDirection(
                    (bp.GetAngularVelocity() / humanoidAgent.physicsManager.maxAngularVelocity).ClampAxis(0, 1)));

            if (bp == humanoidAgent.hips)
            {
                return;
            }
            sensor.AddObservation(bp.transform.localRotation.NormalizeForObservation());
            sensor.AddObservation(bp.currentStrength / humanoidAgent.physicsManager.maximumForce);
        }
        
        private float GetHeadHeight()
        {
            Physics.Raycast(_humanoidAgent.head.transform.position + Vector3.up * 5f,  Vector3.down, out var hitInfo,  Mathf.Infinity, groundLayer);
            return hitInfo.transform == null ? 0f : Vector3.Distance(_humanoidAgent.head.transform.position, hitInfo.point);
        }
    }
}
