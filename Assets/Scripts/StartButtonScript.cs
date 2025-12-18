using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartButtonScript : MonoBehaviour{
	[SerializeField] private TextMeshProUGUI button;
	public void ButtonPressed(){
		button.fontSize = 0f;
		Debug.Log("Clicked");
	}
}
