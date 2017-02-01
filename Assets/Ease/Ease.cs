using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Rage
{
	public enum EaseType
	{
		Linear,
		SineIn, SineOut, SineInOut,
		QuadIn, QuadOut, QuadInOut,
		CubicIn, CubicOut, CubicInOut,
		QuartIn, QuartOut, QuartInOut,
		QuintIn, QuintOut, QuintInOut,
		ExpoIn, ExpoOut, ExpoInOut,
		CircIn, CircOut, CircInOut,
		BackIn, BackOut, BackInOut,
		ElasticIn, ElasticOut, ElasticInOut,
		BounceIn, BounceOut, BounceInOut,
		Spring
	}
	public static class Ease
	{
		private readonly static Dictionary<EaseType, Func<float, float, float, float>> Types = new Dictionary<EaseType, Func<float, float, float, float>>
		{
			{EaseType.Linear, Mathf.Lerp},
			{EaseType.SineIn, SineIn}, {EaseType.SineOut, SineOut}, {EaseType.SineInOut, SineInOut},
			{EaseType.QuadIn, QuadIn}, {EaseType.QuadOut, QuadOut}, {EaseType.QuadInOut, QuadInOut},
			{EaseType.CubicIn, CubicIn}, {EaseType.CubicOut, CubicOut}, {EaseType.CubicInOut, CubicInOut},
			{EaseType.QuartIn, QuartIn}, {EaseType.QuartOut, QuartOut}, {EaseType.QuartInOut, QuartInOut},
			{EaseType.QuintIn, QuintIn}, {EaseType.QuintOut, QuintOut}, {EaseType.QuintInOut, QuintInOut},
			{EaseType.ExpoIn, ExpoIn}, {EaseType.ExpoOut, ExpoOut}, {EaseType.ExpoInOut, ExpoInOut},
			{EaseType.CircIn, CircIn}, {EaseType.CircOut, CircOut}, {EaseType.CircInOut, CircInOut},
			{EaseType.BackIn, BackIn}, {EaseType.BackOut, BackOut}, {EaseType.BackInOut, BackInOut},
			{EaseType.ElasticIn, ElasticIn}, {EaseType.ElasticOut, ElasticOut}, {EaseType.ElasticInOut, ElasticInOut},
			{EaseType.BounceIn, BounceIn}, {EaseType.BounceOut, BounceOut}, {EaseType.BounceInOut, BounceInOut},
			{EaseType.Spring, Spring}
		};
		public static IEnumerator Go(MonoBehaviour m, float from, float to, float time,
			Action<float> update, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoCoroutine(MonoBehaviour m, float from, float to, float time,
			Action<float> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
					update(Types[type](from, to, Mathf.Clamp01(t)));
					yield return null;
				}
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
						update(Types[type](to, from, Mathf.Clamp01(t)));
						yield return null;
					}
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		private static float GetAlpha(Component m)
		{
			var canvasGroup = m.GetComponent<CanvasGroup>();
			if (canvasGroup != null)
				return canvasGroup.alpha;
			var image = m.GetComponent<Image>();
			if (image != null)
				return image.color.a;
			return Camera.main.backgroundColor.a;
		}
		public static IEnumerator GoAlphaTo(MonoBehaviour m, float to, float time,
			Action<float> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			return GoAlpha(m, GetAlpha(m), to, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoAlphaBy(MonoBehaviour m, float by, float time,
			Action<float> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var alpha = GetAlpha(m);
			return GoAlpha(m, alpha, alpha + by, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoAlpha(MonoBehaviour m, float from, float to, float time,
			Action<float> update, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoAlphaCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoAlphaCoroutine(MonoBehaviour m, float from, float to, float time,
			Action<float> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var image = m.GetComponent<Image>();
			var canvasGroup = m.GetComponent<CanvasGroup>();
			Action<float> setAlpha = value =>
			{
				if (canvasGroup != null)
					canvasGroup.alpha = value;
				else if (image != null)
					image.color = image.color.SetAlpha(value);
				else
					Camera.main.backgroundColor = Camera.main.backgroundColor.SetAlpha(value);
			};
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
					var p = Types[type](from, to, Mathf.Clamp01(t));
					setAlpha(p);
					if (update != null)
						update(p);
					yield return null;
				}
				setAlpha(to);
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
						var p = Types[type](to, from, Mathf.Clamp01(t));
						setAlpha(p);
						if (update != null)
							update(p);
						yield return null;
					}
					setAlpha(from);
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		private const float HalfPi = Mathf.PI * .5f;
		private const float DoublePi = Mathf.PI * 2f;
		public static float SineIn(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, 1f - Mathf.Cos(time * HalfPi));
		}
		public static float SineOut(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, Mathf.Sin(time * HalfPi));
		}
		public static float SineInOut(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, .5f * (1f - Mathf.Cos(Mathf.PI * time)));
		}
		public static float QuadIn(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, time * time);
		}
		public static float QuadOut(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, -time * (time - 2f));
		}
		public static float QuadInOut(float from, float to, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(from, to, .5f * time * time);
			return Mathf.Lerp(from, to, -.5f * (((--time) * (time - 2f) - 1f)));
		}
		public static float CubicIn(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, time * time * time);
		}
		public static float CubicOut(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, (time -= 1f) * time * time + 1f);
		}
		public static float CubicInOut(float from, float to, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(from, to, .5f * time * time * time);
			return Mathf.Lerp(from, to, .5f * ((time -= 2) * time * time + 2f));
		}
		public static float QuartIn(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, time * time * time * time);
		}
		public static float QuartOut(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, -((time -= 1f) * time * time * time - 1f));
		}
		public static float QuartInOut(float from, float to, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(from, to, .5f * time * time * time * time);
			return Mathf.Lerp(from, to, -.5f * ((time -= 2f) * time * time * time - 2f));
		}
		public static float QuintIn(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, time * time * time * time * time);
		}
		public static float QuintOut(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, (time -= 1f) * time * time * time * time + 1f);
		}
		public static float QuintInOut(float from, float to, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(from, to, .5f * time * time * time * time * time);
			return Mathf.Lerp(from, to, .5f * ((time -= 2f) * time * time * time * time + 2f));
		}
		public static float ExpoIn(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, Mathf.Pow(2f, 10f * (time - 1f)));
		}
		public static float ExpoOut(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, -Mathf.Pow(2f, -10f * time) + 1f);
		}
		public static float ExpoInOut(float from, float to, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(from, to, .5f * Mathf.Pow(2f, 10f * (time - 1f)));
			return Mathf.Lerp(from, to, .5f * (-Mathf.Pow(2f, -10f * --time) + 2f));
		}
		public static float CircIn(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, -(Mathf.Sqrt(1f - time * time) - 1f));
		}
		public static float CircOut(float from, float to, float time)
		{
			return Mathf.Lerp(from, to, Mathf.Sqrt(1f - (time -= 1f) * time));
		}
		public static float CircInOut(float from, float to, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(from, to, -.5f * (Mathf.Sqrt(1f - time * time) - 1f));
			return Mathf.Lerp(from, to, .5f * (Mathf.Sqrt(1f - (time -= 2f) * time) + 1f));
		}
		public static float BackIn(float from, float to, float time)
		{
			const float s = 1.70158f;
			to -= from;
			return to * time * time * ((s + 1f) * time - s) + from;
		}
		public static float BackOut(float from, float to, float time)
		{
			const float s = 1.70158f;
			to -= from;
			return to * (--time * time * ((s + 1f) * time + s) + 1f) + from;
		}
		public static float BackInOut(float from, float to, float time)
		{
			const float s = 1.70158f * 1.525f;
			to -= from;
			if ((time /= .5f) < 1f)
				return to * .5f * (time * time * ((s + 1f) * time - s)) + from;
			return to * .5f * ((time -= 2) * time * ((s + 1f) * time  + s) + 2f) + from;
		}
		public static float ElasticIn(float from, float to, float time)
		{
			const float p = .3f;
			const float s = p / 4f;
			to -= from;
			return to * -(Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p)) + from;
		}
		public static float ElasticOut(float from, float to, float time)
		{
			const float p = .3f;
			const float s = p / 4f;
			to -= from;
			return to * Mathf.Pow(2f, -10f * time) * Mathf.Sin((time - s) * DoublePi / p) + to + from;
		}
		public static float ElasticInOut(float from, float to, float time)
		{
			const float p = .3f * 1.5f;
			const float s = p / 4f;
			to -= from;
			if ((time /= .5f) < 1f)
				return -.5f * (to * Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p)) + from;
			return to * Mathf.Pow(2f, -10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p) * .5f + to + from;
		}
		public static float BounceIn(float from, float to, float time)
		{
			to -= from;
			return to - BounceOut(0f, to, 1f - time) + from;
		}
		public static float BounceOut(float from, float to, float time)
		{
			to -= from;
			if (time < (1f / 2.75f))
				return to * (7.5625f * time * time) + from;
			if (time < (2f / 2.75f))
				return to * (7.5625f * (time -= (1.5f / 2.75f)) * time + .75f) + from;
			if (time < (2.5f / 2.75f))
				return to * (7.5625f * (time -= (2.25f / 2.75f)) * time + .9375f) + from;
			return to * (7.5625f * (time -= (2.625f / 2.75f)) * time + .984375f) + from;
		}
		public static float BounceInOut(float from, float to, float time)
		{
			to -= from;
			if (time < .5f)
				return BounceIn(0f, to, time * 2f) * .5f + from;
			return BounceOut(0f, to, time * 2f - 1f) * .5f + to * .5f + from;
		}
		public static float Spring(float from, float to, float time)
		{
			time = Mathf.Clamp01(time);
			time = (Mathf.Sin(time * Mathf.PI * (.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
			return from + (to - from) * time;
		}
	}
	public static class Ease3
	{
		private readonly static Dictionary<EaseType, Func<Vector3, Vector3, float, Vector3>> Types = new Dictionary<EaseType, Func<Vector3, Vector3, float, Vector3>>
		{
			{EaseType.Linear, Vector3.Lerp},
			{EaseType.SineIn, (from, to, time) => new Vector3(Ease.SineIn(from.x, to.x, time), Ease.SineIn(from.y, to.y, time), Ease.SineIn(from.z, to.z, time))},
			{EaseType.SineOut, (from, to, time) => new Vector3(Ease.SineOut(from.x, to.x, time), Ease.SineOut(from.y, to.y, time), Ease.SineOut(from.z, to.z, time))},
			{EaseType.SineInOut, (from, to, time) => new Vector3(Ease.SineInOut(from.x, to.x, time), Ease.SineInOut(from.y, to.y, time), Ease.SineInOut(from.z, to.z, time))},
			{EaseType.QuadIn, (from, to, time) => new Vector3(Ease.QuadIn(from.x, to.x, time), Ease.QuadIn(from.y, to.y, time), Ease.QuadIn(from.z, to.z, time))},
			{EaseType.QuadOut, (from, to, time) => new Vector3(Ease.QuadOut(from.x, to.x, time), Ease.QuadOut(from.y, to.y, time), Ease.QuadOut(from.z, to.z, time))},
			{EaseType.QuadInOut, (from, to, time) => new Vector3(Ease.QuadInOut(from.x, to.x, time), Ease.QuadInOut(from.y, to.y, time), Ease.QuadInOut(from.z, to.z, time))},
			{EaseType.CubicIn, (from, to, time) => new Vector3(Ease.CubicIn(from.x, to.x, time), Ease.CubicIn(from.y, to.y, time), Ease.CubicIn(from.z, to.z, time))},
			{EaseType.CubicOut, (from, to, time) => new Vector3(Ease.CubicOut(from.x, to.x, time), Ease.CubicOut(from.y, to.y, time), Ease.CubicOut(from.z, to.z, time))},
			{EaseType.CubicInOut, (from, to, time) => new Vector3(Ease.CubicInOut(from.x, to.x, time), Ease.CubicInOut(from.y, to.y, time), Ease.CubicInOut(from.z, to.z, time))},
			{EaseType.QuartIn, (from, to, time) => new Vector3(Ease.QuartIn(from.x, to.x, time), Ease.QuartIn(from.y, to.y, time), Ease.QuartIn(from.z, to.z, time))},
			{EaseType.QuartOut, (from, to, time) => new Vector3(Ease.QuartOut(from.x, to.x, time), Ease.QuartOut(from.y, to.y, time), Ease.QuartOut(from.z, to.z, time))},
			{EaseType.QuartInOut, (from, to, time) => new Vector3(Ease.QuartInOut(from.x, to.x, time), Ease.QuartInOut(from.y, to.y, time), Ease.QuartInOut(from.z, to.z, time))},
			{EaseType.QuintIn, (from, to, time) => new Vector3(Ease.QuintIn(from.x, to.x, time), Ease.QuintIn(from.y, to.y, time), Ease.QuintIn(from.z, to.z, time))},
			{EaseType.QuintOut, (from, to, time) => new Vector3(Ease.QuintOut(from.x, to.x, time), Ease.QuintOut(from.y, to.y, time), Ease.QuintOut(from.z, to.z, time))},
			{EaseType.QuintInOut, (from, to, time) => new Vector3(Ease.QuintInOut(from.x, to.x, time), Ease.QuintInOut(from.y, to.y, time), Ease.QuintInOut(from.z, to.z, time))},
			{EaseType.ExpoIn, (from, to, time) => new Vector3(Ease.ExpoIn(from.x, to.x, time), Ease.ExpoIn(from.y, to.y, time), Ease.ExpoIn(from.z, to.z, time))},
			{EaseType.ExpoOut, (from, to, time) => new Vector3(Ease.ExpoOut(from.x, to.x, time), Ease.ExpoOut(from.y, to.y, time), Ease.ExpoOut(from.z, to.z, time))},
			{EaseType.ExpoInOut, (from, to, time) => new Vector3(Ease.ExpoInOut(from.x, to.x, time), Ease.ExpoInOut(from.y, to.y, time), Ease.ExpoInOut(from.z, to.z, time))},
			{EaseType.CircIn, (from, to, time) => new Vector3(Ease.CircIn(from.x, to.x, time), Ease.CircIn(from.y, to.y, time), Ease.CircIn(from.z, to.z, time))},
			{EaseType.CircOut, (from, to, time) => new Vector3(Ease.CircOut(from.x, to.x, time), Ease.CircOut(from.y, to.y, time), Ease.CircOut(from.z, to.z, time))},
			{EaseType.CircInOut, (from, to, time) => new Vector3(Ease.CircInOut(from.x, to.x, time), Ease.CircInOut(from.y, to.y, time), Ease.CircInOut(from.z, to.z, time))},
			{EaseType.BackIn, (from, to, time) => new Vector3(Ease.BackIn(from.x, to.x, time), Ease.BackIn(from.y, to.y, time), Ease.BackIn(from.z, to.z, time))},
			{EaseType.BackOut, (from, to, time) => new Vector3(Ease.BackOut(from.x, to.x, time), Ease.BackOut(from.y, to.y, time), Ease.BackOut(from.z, to.z, time))},
			{EaseType.BackInOut, (from, to, time) => new Vector3(Ease.BackInOut(from.x, to.x, time), Ease.BackInOut(from.y, to.y, time), Ease.BackInOut(from.z, to.z, time))},
			{EaseType.ElasticIn, (from, to, time) => new Vector3(Ease.ElasticIn(from.x, to.x, time), Ease.ElasticIn(from.y, to.y, time), Ease.ElasticIn(from.z, to.z, time))},
			{EaseType.ElasticOut, (from, to, time) => new Vector3(Ease.ElasticOut(from.x, to.x, time), Ease.ElasticOut(from.y, to.y, time), Ease.ElasticOut(from.z, to.z, time))},
			{EaseType.ElasticInOut, (from, to, time) => new Vector3(Ease.ElasticInOut(from.x, to.x, time), Ease.ElasticInOut(from.y, to.y, time), Ease.ElasticInOut(from.z, to.z, time))},
			{EaseType.BounceIn, (from, to, time) => new Vector3(Ease.BounceIn(from.x, to.x, time), Ease.BounceIn(from.y, to.y, time), Ease.BounceIn(from.z, to.z, time))},
			{EaseType.BounceOut, (from, to, time) => new Vector3(Ease.BounceOut(from.x, to.x, time), Ease.BounceOut(from.y, to.y, time), Ease.BounceOut(from.z, to.z, time))},
			{EaseType.BounceInOut, (from, to, time) => new Vector3(Ease.BounceInOut(from.x, to.x, time), Ease.BounceInOut(from.y, to.y, time), Ease.BounceInOut(from.z, to.z, time))},
			{EaseType.Spring, (from, to, time) => new Vector3(Ease.Spring(from.x, to.x, time), Ease.Spring(from.y, to.y, time), Ease.Spring(from.z, to.z, time))}
		};
		public static IEnumerator Go(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoCoroutine(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
					update(Types[type](from, to, Mathf.Clamp01(t)));
					yield return null;
				}
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
						update(Types[type](to, from, Mathf.Clamp01(t)));
						yield return null;
					}
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		public static IEnumerator GoPositionTo(MonoBehaviour m, Vector3 to, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			return GoPosition(m, m.transform.localPosition, to, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoPositionBy(MonoBehaviour m, Vector3 by, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var p = m.transform.localPosition;
			return GoPosition(m, p, p + by, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoPosition(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoPositionCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoPositionCoroutine(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
					var p = Types[type](from, to, Mathf.Clamp01(t));
					m.transform.localPosition = p;
					if (update != null)
						update(p);
					yield return null;
				}
				m.transform.localPosition = to;
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
						var p = Types[type](to, from, Mathf.Clamp01(t));
						m.transform.localPosition = p;
						if (update != null)
							update(p);
						yield return null;
					}
					m.transform.localPosition = from;
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		public static IEnumerator GoRotationTo(MonoBehaviour m, Vector3 to, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			return GoRotation(m, m.transform.localEulerAngles, to, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoRotationBy(MonoBehaviour m, Vector3 by, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var p = m.transform.localEulerAngles;
			return GoRotation(m, p, p + by, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoRotation(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoRotationCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoRotationCoroutine(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
					var p = Types[type](from, to, Mathf.Clamp01(t));
					m.transform.localEulerAngles = p;
					if (update != null)
						update(p);
					yield return null;
				}
				m.transform.localEulerAngles = to;
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
						var p = Types[type](to, from, Mathf.Clamp01(t));
						m.transform.localEulerAngles = p;
						if (update != null)
							update(p);
						yield return null;
					}
					m.transform.localEulerAngles = from;
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		public static IEnumerator GoScaleTo(MonoBehaviour m, Vector3 to, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			return GoScale(m, m.transform.localScale, to, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoScaleBy(MonoBehaviour m, Vector3 by, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var p = m.transform.localScale;
			return GoScale(m, p, p + by, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoScale(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoScaleCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoScaleCoroutine(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var last = Time.unscaledTime;
			Func<float> deltaTime = () =>
			{
				var t = Time.unscaledTime;
				var d = t - last;
				last = t;
				return d;
			};
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? deltaTime() : Time.deltaTime) / time;
					var p = Types[type](from, to, Mathf.Clamp01(t));
					m.transform.localScale = p;
					if (update != null)
						update(p);
					yield return null;
				}
				m.transform.localScale = to;
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? deltaTime() : Time.deltaTime) / time;
						var p = Types[type](to, from, Mathf.Clamp01(t));
						m.transform.localScale = p;
						if (update != null)
							update(p);
						yield return null;
					}
					m.transform.localScale = from;
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		private static Color GetColor(MonoBehaviour m)
		{
			var image = m.GetComponent<Image>();
			return (image == null) ? Camera.main.backgroundColor : image.color;
		}
		public static IEnumerator GoColorTo(MonoBehaviour m, Vector3 to, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			return GoColor(m, GetColor(m).GetVector3(), to, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoColorBy(MonoBehaviour m, Vector3 by, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var color = GetColor(m).GetVector3();
			return GoColor(m, color, color + by, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoColor(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoColorCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoColorCoroutine(MonoBehaviour m, Vector3 from, Vector3 to, float time,
			Action<Vector3> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var image = m.GetComponent<Image>();
			var camera = Camera.main;
			Action<Vector3> setColor = value =>
			{
				if (image == null)
					camera.backgroundColor = value.GetColor().SetAlpha(camera.backgroundColor.a);
				else
					image.color = value.GetColor().SetAlpha(image.color.a);
			};
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
					var p = Types[type](from, to, Mathf.Clamp01(t));
					setColor(p);
					if (update != null)
						update(p);
					yield return null;
				}
				setColor(to);
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
						var p = Types[type](to, from, Mathf.Clamp01(t));
						setColor(p);
						if (update != null)
							update(p);
						yield return null;
					}
					setColor(from);
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
	}
	public static class Ease4
	{
		private readonly static Dictionary<EaseType, Func<Vector4, Vector4, float, Vector4>> Types = new Dictionary<EaseType, Func<Vector4, Vector4, float, Vector4>>
		{
			{EaseType.Linear, Vector4.Lerp},
			{EaseType.SineIn, (from, to, time) => new Vector4(Ease.SineIn(from.x, to.x, time), Ease.SineIn(from.y, to.y, time), Ease.SineIn(from.z, to.z, time), Ease.SineIn(from.w, to.w, time))},
			{EaseType.SineOut, (from, to, time) => new Vector4(Ease.SineOut(from.x, to.x, time), Ease.SineOut(from.y, to.y, time), Ease.SineOut(from.z, to.z, time), Ease.SineOut(from.w, to.w, time))},
			{EaseType.SineInOut, (from, to, time) => new Vector4(Ease.SineInOut(from.x, to.x, time), Ease.SineInOut(from.y, to.y, time), Ease.SineInOut(from.z, to.z, time), Ease.SineInOut(from.w, to.w, time))},
			{EaseType.QuadIn, (from, to, time) => new Vector4(Ease.QuadIn(from.x, to.x, time), Ease.QuadIn(from.y, to.y, time), Ease.QuadIn(from.z, to.z, time), Ease.QuadIn(from.w, to.w, time))},
			{EaseType.QuadOut, (from, to, time) => new Vector4(Ease.QuadOut(from.x, to.x, time), Ease.QuadOut(from.y, to.y, time), Ease.QuadOut(from.z, to.z, time), Ease.QuadOut(from.w, to.w, time))},
			{EaseType.QuadInOut, (from, to, time) => new Vector4(Ease.QuadInOut(from.x, to.x, time), Ease.QuadInOut(from.y, to.y, time), Ease.QuadInOut(from.z, to.z, time), Ease.QuadInOut(from.w, to.w, time))},
			{EaseType.CubicIn, (from, to, time) => new Vector4(Ease.CubicIn(from.x, to.x, time), Ease.CubicIn(from.y, to.y, time), Ease.CubicIn(from.z, to.z, time), Ease.CubicIn(from.w, to.w, time))},
			{EaseType.CubicOut, (from, to, time) => new Vector4(Ease.CubicOut(from.x, to.x, time), Ease.CubicOut(from.y, to.y, time), Ease.CubicOut(from.z, to.z, time), Ease.CubicOut(from.w, to.w, time))},
			{EaseType.CubicInOut, (from, to, time) => new Vector4(Ease.CubicInOut(from.x, to.x, time), Ease.CubicInOut(from.y, to.y, time), Ease.CubicInOut(from.z, to.z, time), Ease.CubicInOut(from.w, to.w, time))},
			{EaseType.QuartIn, (from, to, time) => new Vector4(Ease.QuartIn(from.x, to.x, time), Ease.QuartIn(from.y, to.y, time), Ease.QuartIn(from.z, to.z, time), Ease.QuartIn(from.w, to.w, time))},
			{EaseType.QuartOut, (from, to, time) => new Vector4(Ease.QuartOut(from.x, to.x, time), Ease.QuartOut(from.y, to.y, time), Ease.QuartOut(from.z, to.z, time), Ease.QuartOut(from.w, to.w, time))},
			{EaseType.QuartInOut, (from, to, time) => new Vector4(Ease.QuartInOut(from.x, to.x, time), Ease.QuartInOut(from.y, to.y, time), Ease.QuartInOut(from.z, to.z, time), Ease.QuartInOut(from.w, to.w, time))},
			{EaseType.QuintIn, (from, to, time) => new Vector4(Ease.QuintIn(from.x, to.x, time), Ease.QuintIn(from.y, to.y, time), Ease.QuintIn(from.z, to.z, time), Ease.QuintIn(from.w, to.w, time))},
			{EaseType.QuintOut, (from, to, time) => new Vector4(Ease.QuintOut(from.x, to.x, time), Ease.QuintOut(from.y, to.y, time), Ease.QuintOut(from.z, to.z, time), Ease.QuintOut(from.w, to.w, time))},
			{EaseType.QuintInOut, (from, to, time) => new Vector4(Ease.QuintInOut(from.x, to.x, time), Ease.QuintInOut(from.y, to.y, time), Ease.QuintInOut(from.z, to.z, time), Ease.QuintInOut(from.w, to.w, time))},
			{EaseType.ExpoIn, (from, to, time) => new Vector4(Ease.ExpoIn(from.x, to.x, time), Ease.ExpoIn(from.y, to.y, time), Ease.ExpoIn(from.z, to.z, time), Ease.ExpoIn(from.w, to.w, time))},
			{EaseType.ExpoOut, (from, to, time) => new Vector4(Ease.ExpoOut(from.x, to.x, time), Ease.ExpoOut(from.y, to.y, time), Ease.ExpoOut(from.z, to.z, time), Ease.ExpoOut(from.w, to.w, time))},
			{EaseType.ExpoInOut, (from, to, time) => new Vector4(Ease.ExpoInOut(from.x, to.x, time), Ease.ExpoInOut(from.y, to.y, time), Ease.ExpoInOut(from.z, to.z, time), Ease.ExpoInOut(from.w, to.w, time))},
			{EaseType.CircIn, (from, to, time) => new Vector4(Ease.CircIn(from.x, to.x, time), Ease.CircIn(from.y, to.y, time), Ease.CircIn(from.z, to.z, time), Ease.CircIn(from.w, to.w, time))},
			{EaseType.CircOut, (from, to, time) => new Vector4(Ease.CircOut(from.x, to.x, time), Ease.CircOut(from.y, to.y, time), Ease.CircOut(from.z, to.z, time), Ease.CircOut(from.w, to.w, time))},
			{EaseType.CircInOut, (from, to, time) => new Vector4(Ease.CircInOut(from.x, to.x, time), Ease.CircInOut(from.y, to.y, time), Ease.CircInOut(from.z, to.z, time), Ease.CircInOut(from.w, to.w, time))},
			{EaseType.BackIn, (from, to, time) => new Vector4(Ease.BackIn(from.x, to.x, time), Ease.BackIn(from.y, to.y, time), Ease.BackIn(from.z, to.z, time), Ease.BackIn(from.w, to.w, time))},
			{EaseType.BackOut, (from, to, time) => new Vector4(Ease.BackOut(from.x, to.x, time), Ease.BackOut(from.y, to.y, time), Ease.BackOut(from.z, to.z, time), Ease.BackOut(from.w, to.w, time))},
			{EaseType.BackInOut, (from, to, time) => new Vector4(Ease.BackInOut(from.x, to.x, time), Ease.BackInOut(from.y, to.y, time), Ease.BackInOut(from.z, to.z, time), Ease.BackInOut(from.w, to.w, time))},
			{EaseType.ElasticIn, (from, to, time) => new Vector4(Ease.ElasticIn(from.x, to.x, time), Ease.ElasticIn(from.y, to.y, time), Ease.ElasticIn(from.z, to.z, time), Ease.ElasticIn(from.w, to.w, time))},
			{EaseType.ElasticOut, (from, to, time) => new Vector4(Ease.ElasticOut(from.x, to.x, time), Ease.ElasticOut(from.y, to.y, time), Ease.ElasticOut(from.z, to.z, time), Ease.ElasticOut(from.w, to.w, time))},
			{EaseType.ElasticInOut, (from, to, time) => new Vector4(Ease.ElasticInOut(from.x, to.x, time), Ease.ElasticInOut(from.y, to.y, time), Ease.ElasticInOut(from.z, to.z, time), Ease.ElasticInOut(from.w, to.w, time))},
			{EaseType.BounceIn, (from, to, time) => new Vector4(Ease.BounceIn(from.x, to.x, time), Ease.BounceIn(from.y, to.y, time), Ease.BounceIn(from.z, to.z, time), Ease.BounceIn(from.w, to.w, time))},
			{EaseType.BounceOut, (from, to, time) => new Vector4(Ease.BounceOut(from.x, to.x, time), Ease.BounceOut(from.y, to.y, time), Ease.BounceOut(from.z, to.z, time), Ease.BounceOut(from.w, to.w, time))},
			{EaseType.BounceInOut, (from, to, time) => new Vector4(Ease.BounceInOut(from.x, to.x, time), Ease.BounceInOut(from.y, to.y, time), Ease.BounceInOut(from.z, to.z, time), Ease.BounceInOut(from.w, to.w, time))},
			{EaseType.Spring, (from, to, time) => new Vector4(Ease.Spring(from.x, to.x, time), Ease.Spring(from.y, to.y, time), Ease.Spring(from.z, to.z, time), Ease.Spring(from.w, to.w, time))}
		};
		public static IEnumerator Go(MonoBehaviour m, Vector4 from, Vector4 to, float time,
			Action<Vector4> update, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoCoroutine(MonoBehaviour m, Vector4 from, Vector4 to, float time,
			Action<Vector4> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
					update(Types[type](from, to, Mathf.Clamp01(t)));
					yield return null;
				}
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
						update(Types[type](to, from, Mathf.Clamp01(t)));
						yield return null;
					}
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		private static Color GetColor(MonoBehaviour m)
		{
			var image = m.GetComponent<Image>();
			return (image == null) ? Camera.main.backgroundColor : image.color;
		}
		public static IEnumerator GoColorTo(MonoBehaviour m, Vector4 to, float time,
			Action<Vector4> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			return GoColor(m, GetColor(m).GetVector4(), to, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoColorBy(MonoBehaviour m, Vector4 by, float time,
			Action<Vector4> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var color = GetColor(m).GetVector4();
			return GoColor(m, color, color + by, time, update, complete, type, delay, repeat, pingPong, realTime);
		}
		public static IEnumerator GoColor(MonoBehaviour m, Vector4 from, Vector4 to, float time,
			Action<Vector4> update = null, Action complete = null, EaseType type = EaseType.Linear,
			float delay = 0f, int repeat = 1, bool pingPong = false, bool realTime = false)
		{
			var i = GoColorCoroutine(m, from, to, time, update, complete, type, delay, repeat, pingPong, realTime);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoColorCoroutine(MonoBehaviour m, Vector4 from, Vector4 to, float time,
			Action<Vector4> update, Action complete, EaseType type,
			float delay, int repeat, bool pingPong, bool realTime)
		{
			var image = m.GetComponent<Image>();
			var camera = Camera.main;
			Action<Vector4> setColor = value =>
			{
				if (image == null)
					camera.backgroundColor = value.GetColor();
				else
					image.color = value.GetColor();
			};
			var counter = repeat;
			while ((repeat == 0 || repeat == -1) || counter > 0)
			{
				if (delay > 0f)
				{
					if (realTime)
						yield return new WaitForSecondsRealtime(delay);
					else
						yield return new WaitForSeconds(delay);
				}
				var t = 0f;
				while (t <= 1f)
				{
					t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
					var p = Types[type](from, to, Mathf.Clamp01(t));
					setColor(p);
					if (update != null)
						update(p);
					yield return null;
				}
				setColor(to);
				if (pingPong)
				{
					if (delay > 0f)
					{
						if (realTime)
							yield return new WaitForSecondsRealtime(delay);
						else
							yield return new WaitForSeconds(delay);
					}
					t = 0f;
					while (t <= 1f)
					{
						t += (realTime ? Time.unscaledDeltaTime : Time.deltaTime) / time;
						var p = Types[type](to, from, Mathf.Clamp01(t));
						setColor(p);
						if (update != null)
							update(p);
						yield return null;
					}
					setColor(from);
				}
				if (repeat != 0)
					counter--;
				if ((repeat == 0 || repeat == -1) && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
	}
	public static class VectorExtensions
	{
		public static Color GetColor(this Vector3 v)
		{
			return new Color(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y), Mathf.Clamp01(v.z));
		}
		public static Color GetColor(this Vector4 v)
		{
			return new Color(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y), Mathf.Clamp01(v.z), Mathf.Clamp01(v.w));
		}
	}
	public static class ColorExtensions
	{
		public static Vector3 GetVector3(this Color c)
		{
			return new Vector3(c.r, c.g, c.b);
		}
		public static Vector4 GetVector4(this Color c)
		{
			return new Vector4(c.r, c.g, c.b, c.a);
		}
		public static Color SetAlpha(this Color c, float a)
		{
			return new Color(c.r, c.g, c.b, a);
		}
	}
	public class WaitForSecondsRealtime : CustomYieldInstruction
	{
		private float _time;
		public override bool keepWaiting
		{
			get { return Time.unscaledTime < _time; }
		}
		public WaitForSecondsRealtime(float time)
		{
			_time = Time.unscaledTime + time;
		}
	}
}
