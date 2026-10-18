using System.Collections.Generic;
using System.Linq;
using QuanticBrains.Core.Scripts.Extensions;
using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations
{
    /// <summary>
    /// Matches bones positions from one rigged character to another, using their avatar definitions
    /// </summary>
    public class BonesMatcher : MonoBehaviour
    {
        public Avatar sourceAvatar;
        public Avatar followerAvatar;

        public Transform sourceRoot;
        public Transform followerRoot;
        
        private class BoneMatch
        {
            public Transform sourceTransform;
            public Transform followerTransform;
            public Vector3 positionOffset;
            public Vector3 fwd;
            public Vector3 up;

            public void ApplyOffsets()
            {
                positionOffset = sourceTransform.InverseTransformPoint(followerTransform.position);
                fwd = sourceTransform.InverseTransformDirection(followerTransform.forward);
                up = sourceTransform.InverseTransformDirection(followerTransform.up);
            }

        }
        private List<BoneMatch> _boneMatches = new List<BoneMatch>();

        void Awake()
        {
            if (sourceAvatar == null || followerAvatar == null || sourceRoot == null || followerRoot == null)
            {
                Debug.LogError("BonesMatcher is not fully configured. Please assign all needed elements");
                return;
            }
        
            //find source bones
            AddMatchingBoneLink(HumanBodyBones.Hips.ToString());
            AddMatchingBoneLink(HumanBodyBones.Spine.ToString());
            AddMatchingBoneLink(HumanBodyBones.Chest.ToString());
            AddMatchingBoneLink(HumanBodyBones.UpperChest.ToString());
            AddMatchingBoneLink(HumanBodyBones.Head.ToString());
            AddMatchingBoneLink(HumanBodyBones.Neck.ToString());
            AddMatchingBoneLink(HumanBodyBones.LeftUpperLeg.ToString());
            AddMatchingBoneLink(HumanBodyBones.LeftLowerLeg.ToString());
            AddMatchingBoneLink(HumanBodyBones.LeftFoot.ToString());
            AddMatchingBoneLink(HumanBodyBones.LeftToes.ToString());
            AddMatchingBoneLink(HumanBodyBones.RightUpperLeg.ToString());
            AddMatchingBoneLink(HumanBodyBones.RightLowerLeg.ToString());
            AddMatchingBoneLink(HumanBodyBones.RightFoot.ToString());
            AddMatchingBoneLink(HumanBodyBones.RightToes.ToString());
            AddMatchingBoneLink(HumanBodyBones.LeftUpperArm.ToString());
            AddMatchingBoneLink(HumanBodyBones.LeftLowerArm.ToString());
            AddMatchingBoneLink(HumanBodyBones.LeftHand.ToString());
            AddMatchingBoneLink(HumanBodyBones.RightUpperArm.ToString());
            AddMatchingBoneLink(HumanBodyBones.RightLowerArm.ToString());
            AddMatchingBoneLink(HumanBodyBones.RightHand.ToString());
        }

        private void AddMatchingBoneLink(string humanBoneName)
        {
            var sourceBone      = sourceAvatar.humanDescription.human.FirstOrDefault(x => x.humanName.Equals(humanBoneName));
            var followerBone    = followerAvatar.humanDescription.human.FirstOrDefault(x => x.humanName.Equals(humanBoneName));
        
            if (string.IsNullOrEmpty(sourceBone.boneName) || string.IsNullOrEmpty(followerBone.boneName)) return;


            BoneMatch newBoneMatch = new BoneMatch
            {
                sourceTransform = sourceRoot.FirstOrDefault(t => t.name.Equals(sourceBone.boneName)),
                followerTransform = followerRoot.FirstOrDefault(t => t.name.Equals(followerBone.boneName))
            };
            newBoneMatch.ApplyOffsets();
        
            _boneMatches.Add(newBoneMatch);
        }
    
        void LateUpdate()
        {
            foreach (BoneMatch bm in _boneMatches)
            {
                bm.followerTransform.position = bm.sourceTransform.TransformPoint(bm.positionOffset);

                Vector3 newFwd = bm.sourceTransform.TransformDirection(bm.fwd);
                Vector3 newUp = bm.sourceTransform.TransformDirection(bm.up);
                bm.followerTransform.rotation = Quaternion.LookRotation(newFwd, newUp);
            }
        }
    }
}
