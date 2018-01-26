using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MultiMenu : MonoBehaviour
{
	private static MultiMenu _instance;
	public static MultiMenu Instance
	{
		get
		{
			return _instance;
		}
	}

	public GameObject RoomLabel;
	public Transform RoomsParent;
	public Transform PlayerParent;
	public Text Info;
	
	public List<PlayerLabel> otherPlayersLable = new List<PlayerLabel>();
    
	private PhotonView PhotonView;
	private const string playerPrefabName = "PlayerPrefab";
	private const string mainSceneName = "main";
	private List<RoomLabel> rooms = new List<RoomLabel>();
	private GameObject myPlayerLableGO;

	private void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.automaticallySyncScene = true;

		// the following line checks if this client was just created (and not yet online). if so, we connect
		if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
		{
			// Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
			PhotonNetwork.ConnectUsingSettings("0.9");
		}

		// generate a name for this player, if none is assigned yet
		if (String.IsNullOrEmpty(PhotonNetwork.playerName))
		{
			PhotonNetwork.playerName = "Guest" + Random.Range(1, 9999);
		}
		
		//Using to send message
		PhotonView = GetComponent<PhotonView>();
		
		//Hide all panels
		UIAnimation.HideAll();
		
		//Inisial first game state to show corect panel
		GameStateManager.ChangeGameState(GameState.Mainmenu);
	}
	
	private void Update()
	{
		Info.text = PhotonNetwork.connectionStateDetailed.ToString();
	}

    public static void SetPlayerName(string name)
	{
		PhotonNetwork.playerName = name;
	}

	//Call by photon after join lobby
	public void OnJoinedLobby()
	{
		//Debug.Log("dppaaa");
	}

    
	//Call by photon after connect PhotonNetwork.ConnectUsingSettings
	private void OnConnectedToMaster()
	{
		PhotonNetwork.automaticallySyncScene = true;
	}
	
	//Call by photon
	private void OnJoinedRoom()
	{
		CreatePlayerLabel(PhotonNetwork.playerName);
	}

	private void CreatePlayerLabel(string playerName)
	{
		myPlayerLableGO = PhotonNetwork.Instantiate(playerPrefabName, Vector3.zero, Quaternion.identity, 0);
	}

	//Call by photon when room info changes
	private void OnReceivedRoomListUpdate()
	{
		RoomInfo[] _rooms = PhotonNetwork.GetRoomList();
		foreach (RoomInfo info in _rooms)
		{
			int index = rooms.FindIndex(x => x.roomName.text == info.Name);
			if (index == -1)
			{
				RoomLabel roomLabel = Instantiate(RoomLabel, RoomsParent).GetComponent<RoomLabel>();
				roomLabel.roomName.text = info.Name;
				roomLabel.playersCount.text = info.PlayerCount + "/4";
				roomLabel.Update = true;
				rooms.Add(roomLabel);
			}
			else
			{
				rooms[index].playersCount.text = info.PlayerCount + "/4";
				rooms[index].Update = true;
			}
		}
		DeleteOldRooms();
	}

	private void DeleteOldRooms()
	{
		for (int i = rooms.Count - 1; i >= 0; i--)
		{
			if (!rooms[i].Update)
			{
				RoomLabel deletedRoomLabel = rooms[i];
				rooms.Remove(deletedRoomLabel);
				Destroy(deletedRoomLabel.gameObject);
			}
			else
			{
				rooms[i].Update = false;
			}
		}
	}

	public void StartGame()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.LoadLevel(mainSceneName);
			//PhotonView.RPC();
		}
	}
}
