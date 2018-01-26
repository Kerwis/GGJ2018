using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLabel : MonoBehaviour
{

	public InputField InputField;
	
	private PhotonView _photonView;

	private void Start()
	{
		
		_photonView = GetComponent<PhotonView>();

		InputField.interactable = _photonView.isMine;

	}
	
	private void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Init(info.sender.NickName);
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{
		if (stream.isWriting) 
		{ 
			//We own this player: send the others our data 
			stream.SendNext(InputField.text);
		} 
		else
		{ 
			//Network player, receive data 
			SetName((string)stream.ReceiveNext());
		} 
	}

	public void Init(string playerName)
	{
		transform.SetParent(MultiMenu.Instance.PlayerParent);
		transform.localScale = Vector3.one;
		GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		SetName(playerName);
	}

	public void SetName(string name)
	{
		InputField.text = name;
		PhotonNetwork.playerName = name;
	}
}
