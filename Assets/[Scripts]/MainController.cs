using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

	public static Action<int> NextTurn;
	public float _rotationFactor = 1f;

	[SerializeField] private float _turnTime = 1f;
	[SerializeField] private GameObject _mainCameraCenter;

	private float _lastTimeUpdate = Single.MinValue;
	private int _turnCounter;
	private float _rotationSpeedX = 1f;
	private float _rotationSpeedY = 1f;
	
	private Vector3 _centerCurrentRotation;
	private Vector3 _centerTargetRotation;

	private Vector3 _rotationSpeed;

	// Use this for initialization
	void Start()
	{
		NextTurn += HandleNextTurn;
		_centerCurrentRotation = _mainCameraCenter.transform.eulerAngles;
		_centerTargetRotation = _centerCurrentRotation;
	}

	private void HandleNextTurn(int TurnNumber)
	{
		//HandleCamera();
	}

	private void HandleCamera()
	{
		if (Input.GetKey((KeyCode)Stering.Keys.Left))
		{
			_centerTargetRotation.y += -_rotationFactor * _rotationSpeedX;
		}
		if (Input.GetKey((KeyCode)Stering.Keys.Right))
		{
			_centerTargetRotation.y += _rotationFactor * _rotationSpeedX;
		}
		if (Input.GetKey((KeyCode)Stering.Keys.Up))
		{
			//TODO	ruch celownika
		}
		if (Input.GetKey((KeyCode)Stering.Keys.Down))
		{
			//TODO rruch celownika
		}

		_centerCurrentRotation	= Vector3.SmoothDamp(_centerCurrentRotation, _centerTargetRotation, ref _rotationSpeed, Time.smoothDeltaTime);
		_mainCameraCenter.transform.eulerAngles = _centerCurrentRotation;
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
		HandleCamera();

		//TODO check is master server
		if (Time.realtimeSinceStartup > _lastTimeUpdate + _turnTime)
		{
			_turnCounter++;
			if (NextTurn != null)
				NextTurn(_turnCounter);
			_lastTimeUpdate = Time.realtimeSinceStartup;
		}
	}
}
