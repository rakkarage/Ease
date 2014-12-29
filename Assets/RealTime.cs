using UnityEngine;
using System.Collections;
namespace ca.HenrySoftware.Ease
{
	public class RealTime : Singleton<RealTime>
	{
		public float DeltaTime { get; private set; }
		private float _lastTime;
		protected virtual void Awake()
		{
			_lastTime = Time.realtimeSinceStartup;
		}
		protected virtual void Update()
		{
			var time = Time.realtimeSinceStartup;
			DeltaTime = Mathf.Max(0f, time - _lastTime);
			_lastTime = time;
		}
		public IEnumerator WaitForSeconds(float time)
		{
			var counter = 0f;
			while (counter < time)
			{
				yield return null;
				counter += DeltaTime;
			}
		}
	}
}
