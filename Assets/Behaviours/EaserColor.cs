using UnityEngine;
namespace ca.HenrySoftware.Ease
{
	public class EaserColor : Easer<Color>
	{
		public override void Go()
		{
			if (By)
				Ease3.GoColorBy(this, Value.GetVector3(), Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
			else
				Ease3.GoColorTo(this, Value.GetVector3(), Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
		}
	}
}
