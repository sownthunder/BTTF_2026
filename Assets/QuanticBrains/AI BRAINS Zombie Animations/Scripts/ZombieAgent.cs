using System;
using System.Collections;
using System.Linq;
using QuanticBrains.Core.Scripts.Agent;
using QuanticBrains.Core.Scripts.Agent.BrainsManager;
using QuanticBrains.Core.Scripts.Agent.Jointed.Humanoid;
using QuanticBrains.Core.Scripts.Extensions;
using UniRx;
using UniRx.Triggers;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations
{
    public class ZombieAgent : MultiBrains
    { 
        public enum ZombieState
        {
            StandingUp,
            Walking,
            Dead,
            Reviving,
            Unknown
        }
        
        public AgentDefinition standUpAgentDefinition;
        public AgentDefinition walkAgentDefinition;

        public event Action OnStartWalking;
        public event Action OnFall;
        public event Action OnDie;
        public event Action OnRevive;
        public event Action<GameObject> OnReachedTarget;

        [HideInInspector] 
        public ZombieState State { get; private set; } = ZombieState.Unknown;

        private HumanoidAgent _ha;
        private float _initialHeight;
        private const float StandUpThreshold = 0.5f;
        private const float WalkThreshold = 0.65f;
        private Coroutine _walkCor;
        private readonly WaitForSeconds _walkPause = new WaitForSeconds(0.25f);
        
        private BoxCollider _destinationTrigger;

        private LayerMask _walkableLayerMask;
        private float _targetWalkSpeed;
        private LayerMask _obstaclesLayerMask;

        public void Initialize(LayerMask walkableLayerMask, float targetWalkSpeed, LayerMask obstaclesLayerMask)
        {
            _walkableLayerMask = walkableLayerMask;
            _targetWalkSpeed = targetWalkSpeed;
            _obstaclesLayerMask = obstaclesLayerMask;
            
            ManageInitialization();
        }

        private void ManageInitialization()
        {
            InitBrains();
            ((WalkObservations)DefinitionsInstantiated[0].observationsManager).groundLayer      = _walkableLayerMask;
            ((StandupObservations)DefinitionsInstantiated[1].observationsManager).groundLayer   = _walkableLayerMask;

            SetWalkSpeed(_targetWalkSpeed);

            LayerMask walkableAndObstacles = _walkableLayerMask | _obstaclesLayerMask;
            
            _ha.GetComponentsInChildren<RayPerceptionSensorComponent3D>(true)
                .ToList()
                .ForEach(sensor => sensor.RayLayerMask = walkableAndObstacles);
        }
        
        protected override void InitBrains()
        {
            definitions.Add(walkAgentDefinition);
            definitions.Add(standUpAgentDefinition);
            base.InitBrains();

            if (_ha != null) return;
            
            _ha = (HumanoidAgent)Agent;
            _initialHeight = GetHeadHeight();
            SetTarget(this.transform.position + Vector3.up * 3f);
        }

        protected override void Initialize(AgentController ac)
        {
        }

        public void SetTarget(Transform target)
        {
            DestinationTrigger.gameObject.transform.parent = target.transform;
            DestinationTrigger.gameObject.transform.localPosition = Vector3.zero;
            _ha.target = DestinationTrigger.transform;
        }

        public void SetTarget(Vector3 position)
        {
            DestinationTrigger.gameObject.transform.position = position;
            _ha.target = DestinationTrigger.transform;
        }

        public void SetWalkSpeed(float walkSpeed)
        {
            DefinitionsInstantiated[0].TargetWalkingSpeed = walkSpeed;
        }

        public void Kill()
        {
            if (State == ZombieState.Dead) return;

            if (_walkCor != null)
            {
                StopCoroutine(_walkCor);
                _walkCor = null;
            }
            _ha.bodyParts.ForEach(bp => bp.SetJointStrength(-1));
            DisableAllBrains();
            State = ZombieState.Dead;
            OnDie?.Invoke();
        }

        public void Revive()
        {
            if (State == ZombieState.Reviving) return;
                
            ManageInitialization();
            OnRevive?.Invoke();
            State = ZombieState.Reviving;
        }

        private void FixedUpdate()
        {
            //dead already?
            if (State == ZombieState.Dead) return;
            
            //stand up
            if (State != ZombieState.StandingUp && GetHeadHeight() < (_initialHeight * StandUpThreshold))
            {
                EnableStandingState();
                return;
            }

            //walk
            if (State != ZombieState.Walking && GetHeadHeight() > _initialHeight * WalkThreshold)
            {
                EnableWalkingState();
            }
        }

        private void EnableStandingState()
        {
            State = ZombieState.StandingUp;
            if (_walkCor != null)
            {
                StopCoroutine(_walkCor);
                _walkCor = null;
            }

            OnFall?.Invoke();
            ChangeDefinition(DefinitionsInstantiated[1]);
        }

        private void EnableWalkingState()
        {
            State = ZombieState.Walking;
            _walkCor ??= StartCoroutine(StartWalk());
        }

        private IEnumerator StartWalk()
        {
            yield return _walkPause;
            OnStartWalking?.Invoke();
            ChangeDefinition(DefinitionsInstantiated[0]);
            _walkCor = null;
        }

        private float GetHeadHeight()
        {
            var leftHeight      = _ha.leftFoot.transform.position.y;
            var rightHeight     = _ha.rightFoot.transform.position.y;

            return _ha.head.transform.position.y - (leftHeight > rightHeight? rightHeight:leftHeight);
        }

        private BoxCollider DestinationTrigger
        {
            get
            {
                if (_destinationTrigger != null) return _destinationTrigger;
                
                _destinationTrigger = new GameObject("destinationTrigger").AddComponent<BoxCollider>().Also(tr => tr.isTrigger = true);
                _destinationTrigger.OnTriggerEnterAsObservable().Subscribe(collider =>
                {
                    if (collider.gameObject.transform.root != this.gameObject.transform.root) return;
                    OnReachedTarget?.Invoke(_ha.target.gameObject);
                }).AddTo(this);

                return _destinationTrigger;
            }
        }
        
    }

}

