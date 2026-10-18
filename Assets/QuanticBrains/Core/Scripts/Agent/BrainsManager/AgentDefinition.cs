using System.Collections.Generic;
using System.Linq;
using QuanticBrains.Core.Scripts.Extensions;
using Sirenix.OdinInspector;
using Unity.Barracuda;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.BrainsManager
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [DisableInInlineEditors]
    public class AgentDefinition : ScriptableObject
    {
	    [TabGroup("Behaviour")]
	    [Tooltip("The per-agent maximum number of steps.")]
	    [ValidateInput("ValidateSteps", "Max Step zero means no step limit for the training episodes.\n¿Are you sure is what you want?", InfoMessageType.Warning)]
	    public int maxSteps;
	    private bool ValidateSteps(int value)
	    {
		    return value > 0;
	    }
	    [TabGroup("Behaviour")]
	    public string behaviourName;
	    [TabGroup("Behaviour")]
	    public int vectorObservationsSize;
	    [TabGroup("Behaviour")]
	    public int numStackedObservations;
	    [TabGroup("Behaviour")]
	    public int continuousActions;
	    [TabGroup("Behaviour")]
	    public int[] discreteActions;
	    [TabGroup("Behaviour")]
	    public NNModel model;
	    /// <summary>
	    /// The frequency with which the agent requests a decision. A DecisionPeriod of 5 means
	    /// that the Agent will request a decision every 5 Academy steps. /// </summary>
	    [TabGroup("Behaviour")]
	    [Range(1, 20)]
	    [Tooltip("The frequency with which the agent requests a decision. A DecisionPeriod " +
	             "of 5 means that the Agent will request a decision every 5 Academy steps.")]
	    public int decisionPeriod = 5;

	    /// <summary>
	    /// Indicates whether or not the agent will take an action during the Academy steps where
	    /// it does not request a decision. Has no effect when DecisionPeriod is set to 1.
	    /// </summary>
	    [TabGroup("Behaviour")]
	    [Tooltip("Indicates whether or not the agent will take an action during the Academy " +
	             "steps where it does not request a decision. Has no effect when DecisionPeriod " +
	             "is set to 1.")]
	    public bool takeActionsBetweenDecisions;
	    
        [TabGroup("Configuration")] 
		[Range(0.1f, 10)] 
		[SerializeField]
		//The walking speed to try and achieve
		public float targetWalkingSpeed	= 10;
		[TabGroup("Configuration")]
		public float maxWalkingSpeed = 10; //The max walking speed
		public float TargetWalkingSpeed // property
		{
			get => targetWalkingSpeed;
			set => targetWalkingSpeed = Mathf.Clamp(value, .1f, maxWalkingSpeed);
		}
		
		[TabGroup("Configuration")]
		[SerializeField]
		private BehaviourManager _behaviourManager;
		[TabGroup("Configuration")]
		[SerializeField]
		private EnvironmentManager _environmentManager;
		[TabGroup("Configuration")]
		public ActionsManager actionsManager;
		[TabGroup("Configuration")]
		public ObservationsManager observationsManager;
		[TabGroup("Configuration")]
		public HeuristicManager heuristicManager;

	    [Title("Sensors")]
		[TabGroup("Configuration")]
		[SerializeField]
		private List<Sensor> _sensors;
		private List<Sensor> _sensorsInstantiated;

		[HideInInspector]
		public BehaviourManager behaviourManagerInstantiated;
		[HideInInspector]
		public EnvironmentManager environmentManagerInstantiated;
		[HideInInspector]
		public bool isEnabled;

		private BehaviorParameters _behaviorParameters;
		private DecisionRequester _decisionRequester;
		private AgentController _ac;
		
		public void Initialize(AgentController ac)
		{
			isEnabled = false;
			_sensorsInstantiated = _sensors
				.Select(sensor => Instantiate(sensor)
					.Also(s=> s.Initialize(ac))
				).ToList();
			if (_behaviourManager != null) behaviourManagerInstantiated	= Instantiate(_behaviourManager);
			if (_environmentManager != null) environmentManagerInstantiated	= Instantiate(_environmentManager);
			behaviourManagerInstantiated?.Initialize(ac);
			environmentManagerInstantiated?.Initialize(ac);
			
			_ac					= ac;
			_behaviorParameters = ac.GetComponent<BehaviorParameters>();
			_decisionRequester	= ac.GetComponent<DecisionRequester>();
		}
		
		public void Enable()
		{
			if (isEnabled) return;
			
			SetModel();
			_sensorsInstantiated.ForEach(sensor => ManageSwappable(sensor));
			ManageSwappable(actionsManager);
			ManageSwappable(observationsManager);
			ManageSwappable(heuristicManager);
			ManageSwappable(behaviourManagerInstantiated);
			ManageSwappable(environmentManagerInstantiated);
			_ac.LazyInitialize();
			
			isEnabled = true;
		}

		public void Disable()
		{
			if (!isEnabled) return;

			_sensorsInstantiated.ForEach(sensor => ManageSwappable(sensor, false));
			ManageSwappable(actionsManager, false);
			ManageSwappable(observationsManager, false);
			ManageSwappable(heuristicManager, false);
			ManageSwappable(behaviourManagerInstantiated, false);
			ManageSwappable(environmentManagerInstantiated, false);
			_ac.DisableAgent();
			
			isEnabled = false;
		}

		private void ManageSwappable(ScriptableObject so, bool wantEnable = true)
		{
			if (!(so is IHotSwappable swappable)) return;
			
			if (wantEnable)
			{
				swappable.Enable();
				return;
			}
			swappable.Disable();
		}
		
		private void SetModel()
		{
			_ac.MaxStep = maxSteps;
			
			_decisionRequester.DecisionPeriod				= decisionPeriod;
			_decisionRequester.TakeActionsBetweenDecisions	= takeActionsBetweenDecisions;
			
			_behaviorParameters.BrainParameters.VectorObservationSize        = vectorObservationsSize;
			_behaviorParameters.BrainParameters.NumStackedVectorObservations = numStackedObservations;
			_behaviorParameters.BrainParameters.ActionSpec                   = new ActionSpec(continuousActions, discreteActions);
			_behaviorParameters.BehaviorName                                 = behaviourName;
			_behaviorParameters.Model                                        = model;
			_behaviorParameters.BehaviorType = model != null ? BehaviorType.InferenceOnly : BehaviorType.Default;
		}
    }
}
