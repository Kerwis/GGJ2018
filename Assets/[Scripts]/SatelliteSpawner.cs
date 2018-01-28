using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Satelites
{
	public class SatelliteSpawner : MonoBehaviour
	{

		[SerializeField] public GameObject Aim;

		public List<Satelite> MineSatelitesPool;
		[HideInInspector] public int MineSateliteCounter;

		public UnityEvent OnMineSateliteCreate;

		private void OnEnable()
		{
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
				myView.RPC("CreateSatellite", PhotonTargets.All);
			}
		}

		[PunRPC]
		private void CreateSatellite()
		{
			GameObject go = PhotonNetwork.Instantiate("Satelite", Vector3.zero, Quaternion.identity, 0);
			Satelite sat = go.GetComponentInChildren<Satelite>();
			if (myView.isMine)
			{
				MineSatelitesPool.Add(sat);
				MineSateliteCounter++;
			}
			SetupSatelite(sat);
			TrianglePainter.Instance.TurnDraw();
		}

		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				stream.SendNext(MineSatelitesPool);
				stream.SendNext(MineSatelitesPool);
			}
			else
			{
				MineSatelitesPool = (List<Satelite>) stream.ReceiveNext();
				MineSateliteCounter = (int) stream.ReceiveNext();
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
				Debug.Log(MineSateliteCounter);
				OnMineSateliteCreate.Invoke();
			}
		}
		
		void OnDisable()
		{
			OnMineSateliteCreate.RemoveListener(CreateMineSatelite);
		}
		
	}
}