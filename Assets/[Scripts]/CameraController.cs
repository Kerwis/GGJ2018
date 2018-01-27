using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	
	public float _rotationFactor = 1f;
	
	[SerializeField]
	private GameObject _mainCameraCenter;
	[SerializeField] 
	private GameObject _aimPoint;
	
	private float _rotationSpeedX = 1f;
	private float _rotationSpeedY = 1f;
	
	private Vector3 _centerCurrentRotation;
	private Vector3 _centerTargetRotation;
	
	private Vector3 _centerAimCurrentRotation;
	private Vector3 _centerAimTargetRotation;
	

	private Vector3 _rotationSpeed;
	private Vector3 _rotationAimSpeed;

	private void HandleCamera()
	{
		if (Input.GetKey((KeyCode) Stering.Keys.Left))
		{
			_centerTargetRotation.y = _rotationFactor * _rotationSpeedX;
		}
		if (Input.GetKey((KeyCode) Stering.Keys.Right))
		{
			_centerTargetRotation.y = -_rotationFactor * _rotationSpeedX;
		}
		if (Input.GetKey((KeyCode) Stering.Keys.Up))
		{
			_centerAimTargetRotation.x = _rotationFactor * _rotationSpeedY;
		}
		if (Input.GetKey((KeyCode) Stering.Keys.Down))
		{
			_centerAimTargetRotation.x = -_rotationFactor * _rotationSpeedY;
		}

		_centerCurrentRotation = Vector3.SmoothDamp(_centerCurrentRotation, _centerTargetRotation, ref _rotationSpeed,
			Time.smoothDeltaTime);
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
	}
}
