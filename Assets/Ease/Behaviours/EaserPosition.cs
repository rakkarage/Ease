using UnityEngine;
namespace ca.HenrySoftware.Rage
{
	public class EaserPosition : Easer<Vector3>
	{
		public override void Go()
		{
			if (By)
				Ease3.GoPositionBy(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
			else
				Ease3.GoPositionTo(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
		}
	}
}
