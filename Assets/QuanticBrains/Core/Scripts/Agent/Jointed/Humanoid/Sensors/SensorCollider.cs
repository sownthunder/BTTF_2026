using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuanticBrains.Core.Scripts.Agent.BrainsManager;
using QuanticBrains.Core.Scripts.Agent.Jointed.BodyParts;
using QuanticBrains.Core.Scripts.Extensions;
using QuanticBrains.Core.Scripts.Helpers;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.Jointed.Humanoid.Sensors
{
    public class SensorCollider : Sensor, IHotSwappable
    {
        [TabGroup("Collider configuration")]
        public Avatar avatar;
        [TabGroup("Collider configuration")]
        [ValueDropdown("GetAllAvatars", IsUniqueList = true)]
        public List<string> humanParts;
        private IEnumerable GetAllAvatars()
        {
            return EditorHelpers.GetAllAvatarsForDropdown(avatar);
        }
        [TabGroup("Collider configuration")] 
        public bool isAgainstTrigger;

        private HumanoidAgent _ha;
        private List<IDisposable> _disposables;

        public override void Initialize(AgentController agent)
        {
            _ha = (HumanoidAgent)agent;
            _disposables = new List<IDisposable>();
        }

        private void AddLayer(BodyPartBase bp, int layer)
        {
            var current = bp.touchingWith.Value;
            current.Add(layer);
            bp.touchingWith.Value = current;
        }
        
        private void RemoveLayer(BodyPartBase bp, int layer)
        {
            var current = bp.touchingWith.Value;
            current.Remove(layer);
            bp.touchingWith.Value = current;
        }

        public void Enable()
        {
            _ha.bodyParts
                .Where(bodyPart => humanParts.Contains(bodyPart.boneName))
                .ForEach(bp =>
                {
                    
                    if (isAgainstTrigger)
                    {
                        _disposables.Add(bp.transform
                            .OnTriggerEnterAsObservable()
                            .Subscribe(col => AddLayer(bp, col.gameObject.layer))
                            .AddTo(_ha));
                        _disposables.Add(bp.transform
                            .OnTriggerExitAsObservable()
                            .Subscribe(col => RemoveLayer(bp, col.gameObject.layer))
                            .AddTo(_ha));
                        return;
                    }
                    
                    _disposables.Add(bp.transform
                        .OnCollisionEnterAsObservable()
                        .Subscribe(col => AddLayer(bp, col.gameObject.layer))
                        .AddTo(_ha));

                    _disposables.Add(bp.transform
                        .OnCollisionExitAsObservable()
                        .Subscribe(col => RemoveLayer(bp, col.gameObject.layer))
                        .AddTo(_ha));
                });
        }

        public void Disable()
        {
            _disposables.ForEach(disposable => disposable.Dispose());
            _disposables.Clear();
        }
    }
}
