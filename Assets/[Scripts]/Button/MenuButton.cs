using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonMenuType
{
	CreateTable,
	JoinTable,
	SaveSettings,
	BackToMainMenu,
	LeaveRoom,
	ChangePawn,
	StartGame,
}

[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour
{

	public ButtonMenuType Type;

	private Button _button;
	
	protected void Awake()
	{
		_button = GetComponent<Button>();

		switch (Type)
		{
			case ButtonMenuType.CreateTable:
				_button.onClick.AddListener(CreateTable);
				break;
			case ButtonMenuType.JoinTable:
				_button.onClick.AddListener(JoinTable);
				break;
			case ButtonMenuType.SaveSettings:
				_button.onClick.AddListener(SaveSettings);
				break;
			case ButtonMenuType.BackToMainMenu:
				_button.onClick.AddListener(BackToMainMenu);
				break;
			case ButtonMenuType.LeaveRoom:
				_button.onClick.AddListener(LeaveRoom);
				break;
			case ButtonMenuType.ChangePawn:
				_button.onClick.AddListener(ChangePawn);
				break;
			case ButtonMenuType.StartGame:
				_button.onClick.AddListener(StartGame);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void JoinTable()
	{
		GameStateManager.ChangeGameState(GameState.ChooseRoom);
		PhotonNetwork.JoinLobby();
	}

	private void CreateTable()
	{
		//TODO
		GameStateManager.ChangeGameState(GameState.CreateTable);
        
	}

	private void SaveSettings()
	{
		//TODO
		GameStateManager.ChangeGameState(GameState.InLobby);
		RoomManager.CreateRoom();
	}

	private void BackToMainMenu()
	{
		GameStateManager.ChangeGameState(GameState.Mainmenu);
	}
	
	private void LeaveRoom()
	{
		GameStateManager.ChangeGameState(GameState.Mainmenu);
		PhotonNetwork.LeaveRoom();
	}
	
	private void StartGame()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.LoadLevel("main");
		}
	}

	protected virtual void ChangePawn()
	{
		
	}
}
