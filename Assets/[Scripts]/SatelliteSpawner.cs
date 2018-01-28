using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Satelites
{
	public class SatelliteSpawner : MonoBehaviour
	{
        TrianglePainter TP;
		
		[SerializeField] 
		public GameObject Aim;

		public List<Vector2> MineSatelitesCords;
		
		[HideInInspector] 
		public int MineSateliteCounter;

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
			
			
			Debug.Log("Add palyer!!!");

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
            InGameManager.Instance.UpdateTexts("0", "360", "0", "40");
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
            myView.RPC("CheckDrow", PhotonTargets.All);
		}

		[PunRPC]
		private void CheckDrow()
		{			
			TP.TurnDraw();
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
            if (!myView.isMine)
                return;
            if (Input.GetKeyDown(KeyCode.Space) && OnMineSateliteCreate != null)
			{
                InGameManager.Instance.BuySatelite();
                Debug.Log("SPACE");
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