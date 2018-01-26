using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoomNameGenerator : MonoBehaviour {

    private Text nameText;
    public string[] names = {"ETI", "OIO", "BUDO", "BIOLHEM" };

	// Use this for initialization
	void Start () {
        nameText = GetComponent<Text>();
        nameText.text = names[Random.Range(0, names.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
