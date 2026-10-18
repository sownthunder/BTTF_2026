using System;
using System.Collections.Generic;
using QuanticBrains.Core.Scripts.Extensions;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.Jointed.BodyParts
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    public class BodyPartBase : ScriptableObject
    {
        [DisableInPlayMode]
        [DisableInEditorMode]
        public string boneName;
        [HideInInspector]
        public Transform transform;
        
        [DisableInPlayMode]
        [DisableInEditorMode]
        public ReactiveProperty<LayerMask> touchingWith;

        [DisableInPlayMode]
        [DisableInEditorMode]
        public float currentStrength;
        
        [DisableInPlayMode]
        [DisableInEditorMode]
        public bool canCollide;
        protected Rigidbody Rb;
        protected ConfigurableJoint Joint;
        protected PhysicsManager Physics;
        protected Transform Root;

        private bool _isExcluded;
        private List<IDisposable> _disposables;

        public virtual void Initialize(string boneName, Transform root, PhysicsManager physics, bool isExcluded)
        {
            _disposables = new List<IDisposable>();
            this.boneName           = boneName;
            Root                    = root;
            transform               = Root.FirstOrDefault((x) => x.name.Equals(boneName));
            canCollide              = true;
            touchingWith            = new ReactiveProperty<LayerMask>();
            Rb                      = transform.GetComponent<Rigidbody>();

            _isExcluded = isExcluded;
            
            if (_isExcluded)
            {
                return;
            }

            Physics             = physics;
            Joint               = transform.GetComponent<ConfigurableJoint>();
            SetupPhysics();
        }
        
        protected virtual void SetupPhysics()
        {
            Rb.maxAngularVelocity = Physics.maxAngularVelocity;

            if (!Joint)
            {
                return;
            }

            var slerpDrive = Joint.slerpDrive;
            var jd = new JointDrive
            {
                positionSpring  = slerpDrive.positionSpring == 0 ? Physics.maxJointSpring : slerpDrive.positionSpring,
                positionDamper  = slerpDrive.positionDamper == 0 ? Physics.jointDampen : slerpDrive.positionDamper,
                maximumForce    = slerpDrive.maximumForce == 0 ? Physics.maximumForce : slerpDrive.maximumForce
            };
            Joint.slerpDrive = jd;
        }
        /// <summary>
        /// Apply torque according to defined goal `x, y, z` angle and force `strength`.
        /// </summary>
        public virtual void SetJointTargetRotation(float x, float y, float z)
        {
            if (_isExcluded || Joint == null)
            {
                return;
            }
            
            x = (x + 1f) * 0.5f;
            y = (y + 1f) * 0.5f;
            z = (z + 1f) * 0.5f;

            var xRot = Mathf.Lerp(Joint.lowAngularXLimit.limit, Joint.highAngularXLimit.limit, x);
            var yRot = Mathf.Lerp(-Joint.angularYLimit.limit, Joint.angularYLimit.limit, y);
            var zRot = Mathf.Lerp(-Joint.angularZLimit.limit, Joint.angularZLimit.limit, z);

            Joint.targetRotation = Quaternion.Euler(xRot, yRot, zRot);
        }
        
        public virtual void SetJointStrength(float strength)
        {
            if (_isExcluded || Joint == null)
            {
                return;
            }
            
            var rawVal = (strength + 1f) * 0.5f * Physics.maximumForce;
            var slerpDrive = Joint.slerpDrive;
            var jd = new JointDrive
            {
                positionSpring  = slerpDrive.positionSpring == 0 ? Physics.maxJointSpring : slerpDrive.positionSpring,
                positionDamper  = slerpDrive.positionDamper == 0 ? Physics.jointDampen : slerpDrive.positionDamper,
                maximumForce = rawVal
            };
            Joint.slerpDrive = jd;
            currentStrength = jd.maximumForce;
        }
        
        public virtual Vector3 GetVelocity()
        {
            return Rb.velocity;
        }
        
        public virtual Vector3 GetAngularVelocity()
        {
            return Rb.angularVelocity;
        }
        
        public bool IsTouchingWith(LayerMask layerMask)
        {
            return touchingWith.Value.ContainsAny(layerMask);
        }
        

        public void SubscribeToOnTouched(AgentController agent, int layerToCompare, Action callback)
        {
            var layerMask = new LayerMask();
            layerMask |= (1 << layerToCompare);
            SubscribeToOnTouched(agent, layerMask, callback);
        }
        
        public void SubscribeToOnTouched(AgentController agent, LayerMask layerToCompare, Action callback)
        {
            _disposables.Add(touchingWith
                .AsObservable()
                .Subscribe(layerMask =>
                {
                    if (!canCollide || !layerToCompare.ContainsAny(layerMask))
                    {
                        return;
                    }
                    
                        
                    callback();
                })
                .AddTo(agent));
        }
        
        public void SubscribeToOnTouchedExit(Unity.MLAgents.Agent agent, int layerToCompare, Action callback)
        {
            var layerMask = new LayerMask();
            layerMask |= (1 << layerToCompare);
            SubscribeToOnTouchedExit(agent, layerMask, callback);
        }
        
        public void SubscribeToOnTouchedExit(Unity.MLAgents.Agent agent, LayerMask layerToCompare, Action callback)
        {
            _disposables.Add(touchingWith
                .AsObservable()
                .Subscribe(layer =>
                {
                    if (layerToCompare.ContainsAny(layer))
                    {
                        callback();
                    }
                })
                .AddTo(agent));
        }

        public void ClearOnTouchedSubscribes()
        {
            _disposables.ForEach(disposable => disposable.Dispose());
            _disposables.Clear();
        }
    }
}
