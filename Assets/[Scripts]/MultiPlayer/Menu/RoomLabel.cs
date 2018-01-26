using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RoomLabel : MonoBehaviour
{

	public Text roomName;
	public Text playersCount;
	
	[HideInInspector]
	public bool Update;

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(JoinRoom);
	}

	public void JoinRoom()
	{
		PhotonNetwork.JoinRoom(roomName.text);
		GameStateManager.ChangeGameState(GameState.InLobby);
	}

}
