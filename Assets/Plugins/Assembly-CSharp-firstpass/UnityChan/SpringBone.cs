using UnityEngine;

namespace UnityChan
{
	public class SpringBone : MonoBehaviour
	{
		public Transform child;

		public Vector3 boneAxis;

		public float radius;

		public bool isUseEachBoneForceSettings;

		public float stiffnessForce;

		public float dragForce;

		public Vector3 springForce;

		public SpringCollider[] colliders;

		public bool debug;

		public float threshold;

		public float limitedValue;

		private float springLength;

		private Quaternion localRotation;

		private Transform trs;

		private Vector3 currTipPos;

		private Vector3 prevTipPos;

		private static readonly Vector3 gravity;

		private void Awake()
		{
		}

		private SpringManager GetParentSpringManager(Transform t)
		{
			return null;
		}

		private void Start()
		{
		}

		private Vector3 GetBetween(Vector3 prevPos, Vector3 curPos)
		{
			return default(Vector3);
		}

		public void UpdateSpring()
		{
		}

		public void ResetTipPos()
		{
		}

		private void OnDrawGizmos()
		{
		}
	}
}
