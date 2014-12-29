using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Ease
{
	public abstract class Easer<T> : MonoBehaviour
	{
		[SerializeField] protected bool Begin = true;
		[SerializeField] protected bool By = true;
		[SerializeField] protected T Value;
		[SerializeField] protected EaseType Type = EaseType.Linear;
		[SerializeField] protected float Time = 1f;
		[SerializeField] protected float Delay = 0f;
		[SerializeField] protected int Repeat = 1;
		[SerializeField] protected bool PingPong = false;
		[SerializeField] protected bool RealTime = false;
		[SerializeField] protected UnityEvent Complete;
		private void Start()
		{
			if (Begin) Go();
		}
		public abstract void Go();
	}
}
