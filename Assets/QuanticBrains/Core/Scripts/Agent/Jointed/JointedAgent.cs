using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuanticBrains.Core.Scripts.Agent.Jointed.BodyParts;
using QuanticBrains.Core.Scripts.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.Jointed
{
    public abstract class JointedAgent : AgentController
    {
        [TabGroup("Configuration")]
        public Avatar avatar;
        [TabGroup("Configuration")]
        [ValueDropdown("GetAllAvatars", IsUniqueList = true)]
        public List<string> excludePartsFromPhysicsSetUp;
        private IEnumerable GetAllAvatars()
        {
            return EditorHelpers.GetAllAvatarsForDropdown(avatar);
        }
		
        [Title("Managers")]
        [TabGroup("Configuration")]
        [SerializeField]
        private BodyPartBase _bodyPart;
        [TabGroup("Configuration")]
        public PhysicsManager physicsManager;
        
        [HideInInspector]
        public List<BodyPartBase> bodyParts;
        
        //This will be used as a stabilized model space reference point for observations
        //Because ragdolls can move erratically during training, using a stabilized reference transform improves learning
        [HideInInspector]
        public OrientationHelperController orientationHelper;

        public override void Initialize()
        {
            base.Initialize();
            orientationHelper = GetComponentInChildren<OrientationHelperController>();
        }

        protected override void FixedUpdate()
        {
            UpdateOrientationObjects();
            base.FixedUpdate();
        }

        public override void PrepareAgent()
        {
            ConfigureParts();
        }

        protected BodyPartBase ManageBodyPart(string humanBone)
        {
            var currentHumanBone = avatar.humanDescription.human.Any(x => x.humanName.Equals(humanBone));
            if (!currentHumanBone)
            {
                return null;
            }

            var bone = avatar.humanDescription.human
                .First(x => x.humanName.Equals(humanBone));
            var bp = Instantiate(_bodyPart);
            bp.Initialize(bone.boneName, transform, physicsManager, excludePartsFromPhysicsSetUp.Contains(bone.boneName));
            bodyParts.Add(bp);
            return bp;
        }

        public override void OnEpisodeBegin()
        {
            UpdateOrientationObjects();
            base.OnEpisodeBegin();
        }

        public abstract Vector3 GetAvgVelocity();
        
        public abstract void UpdateOrientationObjects();
		
        public abstract void ConfigureParts();
    }
}
