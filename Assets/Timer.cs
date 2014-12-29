using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Ease
{
	public class Timer : MonoBehaviour
	{
		[SerializeField] private Image _slowStatus = null;
		private bool _slow;
		[SerializeField] private Image _pauseStatus = null;
		private bool _pause;
		[SerializeField] private Image _fastStatus = null;
		private bool _fast;
		private readonly Color _colorOff = Color.white.SetAlpha(.75f);
		private readonly Color _colorOn = Color.blue.SetAlpha(.75f);
		private readonly Vector3 _scaleTo = new Vector3(1.333f, 1.333f, 1f);
		private const float _time = .333f;
		private void Start()
		{
			_slowStatus.color = _colorOff;
			_pauseStatus.color = _colorOff;
			_fastStatus.color = _colorOff;
		}
		public void Slow()
		{
			_pause = false;
			_pauseStatus.color = _colorOff;
			_fast = false;
			_fastStatus.color = _colorOff;
			Time.timeScale = (_slow = !_slow) ? .1f : 1f;
			_slowStatus.StopAllCoroutines();
			Ease3.GoColorTo(_slowStatus, (_slow ? _colorOn : _colorOff).GetVector3(), _time, null, null, EaseType.SineInOut, 0f, 1, false, true);
			Ease3.GoScaleTo(_slowStatus, _scaleTo, _time, null, () =>
			{
				Ease3.GoScaleTo(_slowStatus, Vector3.one, _time, null, null, EaseType.BackInOut, 0f, 1, false, true);
			}, EaseType.BackInOut, 0f, 1, false, true);
		}
		public void Pause()
		{
			Time.timeScale = (_pause = !_pause) ? 0f : _slow ? .1f : _fast ? 2f : 1f;
			_pauseStatus.StopAllCoroutines();
			Ease3.GoColorTo(_pauseStatus, (_pause ? _colorOn : _colorOff).GetVector3(), _time, null, null, EaseType.SineInOut, 0f, 1, false, true);
			Ease3.GoScaleTo(_pauseStatus, _scaleTo, _time, null, () =>
			{
				Ease3.GoScaleTo(_pauseStatus, Vector3.one, _time, null, null, EaseType.BackInOut, 0f, 1, false, true);
			}, EaseType.BackInOut, 0f, 1, false, true);
		}
		public void Fast()
		{
			_slow = false;
			_slowStatus.color = _colorOff;
			_pause = false;
			_pauseStatus.color = _colorOff;
			Time.timeScale = (_fast = !_fast) ? 2f : 1f;
			_fastStatus.StopAllCoroutines();
			Ease3.GoColorTo(_fastStatus, (_fast ? _colorOn : _colorOff).GetVector3(), _time, null, null, EaseType.SineInOut, 0f, 1, false, true);
			Ease3.GoScaleTo(_fastStatus, _scaleTo, _time, null, () =>
			{
				Ease3.GoScaleTo(_fastStatus, Vector3.one, _time, null, null, EaseType.BackInOut, 0f, 1, false, true);
			}, EaseType.BackInOut, 0f, 1, false, true);
		}
	}
}
