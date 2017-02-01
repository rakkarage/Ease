using UnityEngine;
namespace ca.HenrySoftware.Rage
{
	public class EaserScale : Easer<Vector3>
	{
		public override void Go()
		{
			if (By)
				Ease3.GoScaleTo(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
			else
				Ease3.GoScaleBy(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
		}
	}
}
