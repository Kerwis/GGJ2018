using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

	public static Action<int> NextTurn;

	[SerializeField] 
	private float _turnTime = .1f;
	[SerializeField] 
	private PhotonView  myView;

	private float _lastTimeUpdate = Single.MinValue;
	private int _turnCounter;

	private readonly string _mainPlayerPath = "Center";
	

	// Use this for initialization
	void Start()
	{
		
		NextTurn += HandleNextTurn;

		SpownPlayer();
	}

	private void SpownPlayer()
	{
		if (PhotonNetwork.isMasterClient)
		{
			myView.RPC("CreatePlayer", PhotonTargets.All);
		}
		else if(!PhotonNetwork.connected)
		{
			Instantiate(Resources.Load(_mainPlayerPath));
		}
	}

	private void HandleNextTurn(int TurnNumber)
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (PhotonNetwork.isMasterClient|| !PhotonNetwork.connected)
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

	[PunRPC]
	private void CreatePlayer()
	{
		PhotonNetwork.Instantiate(_mainPlayerPath, Vector3.zero, Quaternion.identity, 0);
	}
}
