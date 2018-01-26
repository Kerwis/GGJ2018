using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MyButton : MonoBehaviour {

	internal Button _button;

	public virtual void Awake()
	{
		_button = GetComponent<Button>();
	}
}
