﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Satelites
{
	public class SatelliteSpawner : MonoBehaviour
	{
        TrianglePainter TP;
		[SerializeField] public GameObject Aim;

		public List<Vector2> MineSatelitesCords;
		[HideInInspector] public int MineSateliteCounter;

		public UnityEvent OnMineSateliteCreate;

		public Cash myCash;

		private void Start()
		{
            TP = GameObject.Find("MainController").GetComponentInChildren<TrianglePainter>();
            myCash = GetComponent<Cash>();
			
			#region EventsInit

			if (OnMineSateliteCreate == null)
				OnMineSateliteCreate = new UnityEvent();

			OnMineSateliteCreate.AddListener(CreateMineSatelite);

			if (myView.isMine)
			{
				SatMenager.mySatelliteSpawners = this;
			}
			else
			{
				SatMenager.enemySatelliteSpawners = this;
				Debug.Log("Add enemy!!!");
			}

			#endregion
		}


		public PhotonView myView;

		void CreateMineSatelite()
		{
			//TODO Add check max satelite
			Debug.Log("add " + myView.isMine);
			if (myView.isMine)
			{
				CreateSatellite();
			}
            
                TP.TurnDraw();
            //else
               // TrianglePainter.Instance = new TrianglePainter();
		}

		[PunRPC]
		private void CreateSatellite()
		{
			GameObject go = PhotonNetwork.Instantiate("Satelite", Vector3.zero, Quaternion.identity, 0);
			Satelite sat = go.GetComponentInChildren<Satelite>();
			SetupSatelite(sat);		
			MineSateliteCounter++;
			MineSatelitesCords.Add(sat.cords);
		}

		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				stream.SendNext(MineSateliteCounter);
				SendList(MineSatelitesCords, stream);
			}
			else
			{				
				MineSateliteCounter = (int) stream.ReceiveNext();
				MineSatelitesCords = ReciveList<Vector2>(stream);
			}
		}

		private List<T> ReciveList <T> (PhotonStream stream)
		{
			
			List<T> tmp = new List<T>();
			for (int i = 0; i < MineSateliteCounter; i++)
			{
				tmp.Add((T)stream.ReceiveNext());
			}
			return tmp;
		}

		private void SendList(List<Vector2> listToSend, PhotonStream stream)
		{
			for (int i = 0; i < MineSateliteCounter; i++)
			{
				stream.SendNext(listToSend[i]);
			}
		}


		private void SetupSatelite(Satelite sat)
		{
			sat.transform.parent.transform.rotation = Aim.transform.rotation;
			sat.Setup();
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space) && OnMineSateliteCreate != null)
			{
				OnMineSateliteCreate.Invoke();
                InGameManager.Instance.BuySatelite();
			}
			if (Input.GetKeyDown((KeyCode.R)))
			{
				SceneManager.UnloadSceneAsync("main");
				SceneManager.LoadSceneAsync("main");
			}
		}
		
		void OnDisable()
		{
			OnMineSateliteCreate.RemoveListener(CreateMineSatelite);
		}
		
	}
}