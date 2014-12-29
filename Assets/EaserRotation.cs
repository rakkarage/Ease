using UnityEngine;
namespace ca.HenrySoftware.Ease
{
	public class EaserRotation : Easer<Vector3>
	{
		public override void Go()
		{
			if (By)
				Ease3.GoRotationTo(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
			else
				Ease3.GoRotationBy(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
		}
	}
}
