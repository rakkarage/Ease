using UnityEngine;
namespace ca.HenrySoftware.Ease
{
	public class EaserColorAlpha : Easer<Color>
	{
		public override void Go()
		{
			if (By)
				Ease4.GoColorBy(this, Value.GetVector4(), Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
			else
				Ease4.GoColorTo(this, Value.GetVector4(), Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
		}
	}
}
