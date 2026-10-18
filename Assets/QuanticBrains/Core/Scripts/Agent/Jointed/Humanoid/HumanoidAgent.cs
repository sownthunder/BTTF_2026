using System.Collections.Generic;
using QuanticBrains.Core.Scripts.Agent.Jointed.BodyParts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.Jointed.Humanoid
{
    public class HumanoidAgent : JointedAgent
    {
	    [HideInInspector] 
	    public BodyPartBase hips;
	    
		[HideInInspector] 
		public BodyPartBase spine;
		
		[HideInInspector] 
		public BodyPartBase upperChest;
		
		[HideInInspector] 
		public BodyPartBase head;
		
		[HideInInspector] 
		public BodyPartBase leftUpperLeg;
		
		[HideInInspector] 
		public BodyPartBase leftLowerLeg;
		
		[HideInInspector] 
		public BodyPartBase leftFoot;
		
		[HideInInspector] 
		public BodyPartBase leftFootToe;
		
		[HideInInspector] 
		public BodyPartBase rightUpperLeg;
		
		[HideInInspector] 
		public BodyPartBase rightLowerLeg;
		
		[HideInInspector] 
		public BodyPartBase rightFoot;
		
		[HideInInspector] 
		public BodyPartBase rightFootToe;
		
		[HideInInspector] 
		public BodyPartBase leftUpperArm;
		
		[HideInInspector] 
		public BodyPartBase leftLowerArm;
		
		[HideInInspector] 
		public BodyPartBase leftHand;
		
		[HideInInspector] 
		public BodyPartBase rightUpperArm;
		
		[HideInInspector] 
		public BodyPartBase rightLowerArm;
		
		[HideInInspector] 
		public BodyPartBase rightHand;
		


	    [HideInInspector]
		public bool IsReady = true;

		public override void ConfigureParts()
		{
			bodyParts = new List<BodyPartBase>();
			
			hips			= ManageBodyPart(HumanBodyBones.Hips.ToString());
			spine			= ManageBodyPart(HumanBodyBones.Spine.ToString());
			upperChest		= ManageBodyPart(HumanBodyBones.UpperChest.ToString());
			head			= ManageBodyPart(HumanBodyBones.Head.ToString());
			leftUpperLeg 	= ManageBodyPart(HumanBodyBones.LeftUpperLeg.ToString());
			leftLowerLeg 	= ManageBodyPart(HumanBodyBones.LeftLowerLeg.ToString());
			leftFoot		= ManageBodyPart(HumanBodyBones.LeftFoot.ToString());
			leftFootToe		= ManageBodyPart(HumanBodyBones.LeftToes.ToString());
			rightUpperLeg	= ManageBodyPart(HumanBodyBones.RightUpperLeg.ToString());
			rightLowerLeg	= ManageBodyPart(HumanBodyBones.RightLowerLeg.ToString());
			rightFoot		= ManageBodyPart(HumanBodyBones.RightFoot.ToString());
			rightFootToe	= ManageBodyPart(HumanBodyBones.RightToes.ToString());
			leftUpperArm 	= ManageBodyPart(HumanBodyBones.LeftUpperArm.ToString());
			leftLowerArm 	= ManageBodyPart(HumanBodyBones.LeftLowerArm.ToString());
			leftHand		= ManageBodyPart(HumanBodyBones.LeftHand.ToString());
			rightUpperArm	= ManageBodyPart(HumanBodyBones.RightUpperArm.ToString());
			rightLowerArm	= ManageBodyPart(HumanBodyBones.RightLowerArm.ToString());
			rightHand		= ManageBodyPart(HumanBodyBones.RightHand.ToString());
		}

		public override void UpdateOrientationObjects()
		{
			orientationHelper.UpdateOrientation(hips.transform, target);
		}

		//Returns the average velocity of all of the body parts
		//Using the velocity of the hips only has shown to result in more erratic movement from the limbs, so...
		//...using the average helps prevent this erratic movement
		public override Vector3 GetAvgVelocity()
		{
			var velSum = Vector3.zero;
			bodyParts.ForEach((bodyPart) => velSum += bodyPart.GetVelocity());
			return velSum / bodyParts.Count;
		}
    }
}
