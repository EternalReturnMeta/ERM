using System;
using UnityEngine;

namespace UnityChan
{
	public class SpringManager : MonoBehaviour
	{
		public float dynamicRatio;

		public float stiffnessForce;

		public AnimationCurve stiffnessCurve;

		public float dragForce;

		public AnimationCurve dragCurve;

		public SpringBone[] springBones;

		public Func<bool> IsRendering;

		public float TestLimitedValue;

		private void LateUpdate()
		{
		}

		[ContextMenu("SpringBonesPropertyCheck")]
		private void SpringBonesChildCheck()
		{
		}

		[ContextMenu("SpringBonesAxisChange")]
		private void SpringBonesAxisChange()
		{
		}

		[ContextMenu("SpringBonesLimitedChange")]
		private void SpringBonesLimitedChange()
		{
		}

		private void OnEnable()
		{
		}

		private void ResetBoneTipPos()
		{
		}
	}
}
