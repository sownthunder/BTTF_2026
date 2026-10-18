using System;
using System.Collections.Generic;
using QuanticBrains.Core.Scripts.Agent.BrainsManager;
using Sirenix.OdinInspector;
using UniRx;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent
{
	
	public abstract class AgentController : Unity.MLAgents.Agent
	{
		[TabGroup("Configuration")]
		public Transform target; //Target the agent will walk towards during training.


		[HideInInspector]
		public AgentDefinition agentDefinition;
		private bool _isInitialized;

		public override void Initialize()
		{
			if (_isInitialized) return;
			_isInitialized = true;
		}

		public abstract void PrepareAgent();

		public override void Heuristic(in ActionBuffers actionsOut)
		{
			agentDefinition.heuristicManager?.Manage(this, actionsOut);
		}

		/// <summary>
		/// Loop over body parts and reset them to initial conditions.
		/// </summary>
		public override void OnEpisodeBegin()
		{
			agentDefinition.environmentManagerInstantiated?.BeginEpisode(this);
		}

		/// <summary>
		/// Loop over body parts to add them to observation.
		/// </summary>
		public override void CollectObservations(VectorSensor sensor)
		{
			if (sensor == null) return;
			agentDefinition.observationsManager?.Collect(this, sensor);
		}

		public override void OnActionReceived(ActionBuffers actionBuffers)
		{
			agentDefinition.actionsManager?.Receive(this, -1, actionBuffers.ContinuousActions, actionBuffers.DiscreteActions);
		}

		protected virtual void FixedUpdate()
		{
		}
		
		protected override void OnEnable()
		{
		}

		public void DisableAgent()
		{
			OnDisable();
		}
	}
}
