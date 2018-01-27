using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

	public static Action<int> NextTurn;
	public float _rotationFactor = 1f;

	[SerializeField] 
	private float _turnTime = 1f;
	[SerializeField]
	private GameObject _mainCameraCenter;
	[SerializeField] 
	private GameObject _aimPoint;
	[SerializeField] 
	private PhotonView myView;

	private float _lastTimeUpdate = Single.MinValue;
	private int _turnCounter;
	private float _rotationSpeedX = 1f;
	private float _rotationSpeedY = 1f;
	
	private Vector3 _centerCurrentRotation;
	private Vector3 _centerTargetRotation;
	
	private Vector3 _centerAimCurrentRotation;
	private Vector3 _centerAimTargetRotation;
	

	private Vector3 _rotationSpeed;
	private Vector3 _rotationAimSpeed;
	

	// Use this for initialization
	void Start()
	{
		
		NextTurn += HandleNextTurn;
	}

	private void HandleNextTurn(int TurnNumber)
	{
		//HandleCamera();
	}

	private void HandleCamera()
	{
		if (Input.GetKey((KeyCode)Stering.Keys.Left))
		{
			_centerTargetRotation.y = _rotationFactor * _rotationSpeedX;
		}
		if (Input.GetKey((KeyCode)Stering.Keys.Right))
		{
			_centerTargetRotation.y = -_rotationFactor * _rotationSpeedX;
		}
		if (Input.GetKey((KeyCode)Stering.Keys.Up))
		{
			_centerAimTargetRotation.x = _rotationFactor * _rotationSpeedY;
        }
		if (Input.GetKey((KeyCode)Stering.Keys.Down))
		{
			_centerAimTargetRotation.x = -_rotationFactor * _rotationSpeedY;
        }

		_centerCurrentRotation	= Vector3.SmoothDamp(_centerCurrentRotation, _centerTargetRotation, ref _rotationSpeed, Time.smoothDeltaTime);
		_centerAimCurrentRotation = Vector3.SmoothDamp(_centerAimCurrentRotation, _centerAimTargetRotation,
			ref _rotationAimSpeed, Time.smoothDeltaTime);
		
		
		_aimPoint.transform.Rotate(_centerAimCurrentRotation);
		_mainCameraCenter.transform.Rotate(_centerCurrentRotation);

		if (_centerCurrentRotation.AlmostEquals(_centerTargetRotation, 1f))
		{
			_centerCurrentRotation = Vector3.zero;
			_centerTargetRotation = Vector3.zero;
		}
		
		if (_centerAimCurrentRotation.AlmostEquals(_centerAimTargetRotation, 1f))
		{
			_centerAimCurrentRotation = Vector3.zero;
			_centerAimTargetRotation = Vector3.zero;
		}
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
		HandleCamera();

		if (PhotonNetwork.isMasterClient)
		{
			if (Time.realtimeSinceStartup > _lastTimeUpdate + _turnTime)
			{
				_turnCounter++;
				if (NextTurn != null)
					NextTurn(_turnCounter);
				//TODO send RPC
				_lastTimeUpdate = Time.realtimeSinceStartup;
			}
		}
	}
}
