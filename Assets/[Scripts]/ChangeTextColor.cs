﻿ using UnityEngine;
 using UnityEngine.UI; // needed to access the new UI classes and functions
 using System.Collections;
 public class ChangeTextColor : MonoBehaviour
 {
     public Text t;
	 
	    public void ChangeColour()
	    {
		t.color = Color.black;
		} 
}
