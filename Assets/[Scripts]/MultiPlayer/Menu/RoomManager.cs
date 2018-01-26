using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    private static RoomManager _instance;

    private static RoomManager instance
    {
        get
        {
            if (_instance == null)
            _instance = FindObjectOfType<RoomManager>();
            return _instance;
        }
    }

    public Text RoomName;
    public Text UserName;

    public static void CreateRoom()
    {      
        RoomOptions options = new RoomOptions() {MaxPlayers = 4, IsOpen = true, IsVisible = true};
        PhotonNetwork.CreateRoom(instance.RoomName.text, options, TypedLobby.Default);
        
        MultiMenu.SetPlayerName(instance.UserName.text);
    }
}
