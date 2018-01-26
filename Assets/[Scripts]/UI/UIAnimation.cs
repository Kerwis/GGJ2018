using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class UIAnimation : MonoBehaviour
{

	public GameState[] ShowOnGameStates;
	public Vector2 ShowPosition;
	public Vector2 HidePosition;
	public bool HideWithAlpha;
	public float Duration = 1f;
	public float Delay = 0f;
	public bool HideNow;
	public bool ShowNow;
	
	private RectTransform _rectTransform;
	private CanvasGroup _canvasGroup;
	private bool _isHide = false;
	private bool _isShow = true;
	private static List<UIAnimation> _activeInstance = new List<UIAnimation>();

	public static void HideAll()
	{
		foreach (UIAnimation uiAnimation in _activeInstance)
		{
			uiAnimation._rectTransform.anchoredPosition = uiAnimation.HidePosition;
			uiAnimation._isHide = true;
		}
	}
	
	
	private void Awake()
	{
		GameStateManager.GameStateChange += StateChange;
		_rectTransform = GetComponent<RectTransform>();
		_canvasGroup = GetComponent<CanvasGroup>();
		_activeInstance.Add(this);
	}

	private void OnDestroy()
	{
		GameStateManager.GameStateChange -= StateChange;
	}

	private void StateChange(GameState state)
	{
		if (ShowOnGameStates.Contains(state))
		{
			Show();
		}
		else
		{
			if (!_isHide)
			{
				Hide();
			}
		}
	}

	private void Show()
	{
		_isHide = false;
		if(ShowNow)
			StartCoroutine(_show(0));
		else
			StartCoroutine(_show(Duration));
	}
	
	private void Hide()
	{
		_isHide = true;
		if(HideNow)
			StartCoroutine(_show(0));
		else
			StartCoroutine(_hide(Duration));
	}

	private IEnumerator _show(float time)
	{
		yield return new WaitForSeconds(Delay);
		float starTime = Time.realtimeSinceStartup;
		float endTime = starTime + time;
		float normalizeTime = (Time.realtimeSinceStartup - starTime) / time;
		while (endTime > Time.realtimeSinceStartup)
		{
			normalizeTime = (Time.realtimeSinceStartup - starTime) / time;
			_rectTransform.anchoredPosition = Vector2.Lerp(HidePosition, ShowPosition, normalizeTime);
			_canvasGroup.alpha = Mathf.Lerp(0, 1, normalizeTime);
			yield return new WaitForEndOfFrame();
		}
		_rectTransform.anchoredPosition = ShowPosition;
	}
	
	private IEnumerator _hide(float time)
	{
		yield return new WaitForSeconds(Delay);
		float starTime = Time.realtimeSinceStartup;
		float endTime = starTime + time;
		float normalizeTime = (Time.realtimeSinceStartup - starTime) / time;
		while (endTime > Time.realtimeSinceStartup)
		{
			normalizeTime = (Time.realtimeSinceStartup - starTime) / time;
			_rectTransform.anchoredPosition = Vector2.Lerp(ShowPosition, HidePosition, normalizeTime);
			_canvasGroup.alpha = Mathf.Lerp(1, 0, normalizeTime);
			yield return new WaitForEndOfFrame();
		}
		_rectTransform.anchoredPosition = HidePosition;
	}
	
}
