using UnityEngine;
namespace ca.HenrySoftware.Ease
{
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
}
