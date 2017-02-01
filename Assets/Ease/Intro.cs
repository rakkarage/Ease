using ca.HenrySoftware.Rage;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]
public class Intro : MonoBehaviour
{
	public Image Henry;
	public Button Fore;
	AudioSource _source;
	const float _timeAnimation = 1.0f;
	const float _timeDelay = .5f;
	const float _timeDelaySound = .8f;
	void Awake()
	{
		_source = GetComponent<AudioSource>();
	}
	void Start()
	{
		var scale = 2;
		var camera = Camera.main;
		if (camera != null)
		{
			var fitX = (int)(Screen.width / Henry.sprite.rect.width);
			var fitY = (int)(Screen.height / Henry.sprite.rect.height);
			var newScale = Mathf.Min(fitX, fitY) - 2;
			if (newScale > scale)
				scale = newScale;
		}
		StartCoroutine(PlayDelayed());
		Ease3.GoScaleTo(Henry, new Vector3(scale, scale, 1f), _timeAnimation, null, null, EaseType.BounceOut, _timeDelay);
		Ease3.GoRotationTo(Henry, new Vector3(0f, 0f, 180f), _timeAnimation, null, null, EaseType.BounceOut, _timeDelay);
		Ease3.GoColorTo(this, Color.black.GetVector3(), _timeAnimation, null, Fade, EaseType.BounceOut, _timeDelay);
	}
	void Done()
	{
		SceneManager.LoadSceneAsync(1);
	}
	IEnumerator PlayDelayed()
	{
		yield return new WaitForSeconds(_timeDelaySound);
		_source.Play();
	}
	void Fade()
	{
		Ease.GoAlpha(Fore, 0f, 1f, _timeAnimation, null, Done, EaseType.Linear, _timeAnimation);
	}
}
