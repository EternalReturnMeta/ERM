using UnityEngine;

namespace UnityChan
{
	public class SpringCollider : MonoBehaviour
	{
		public float radius;

		private Transform cachedTransform;

		private bool transformCached;

		public Vector3 position => default(Vector3);

		private void OnDrawGizmosSelected()
		{
		}
	}
}
