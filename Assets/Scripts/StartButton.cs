using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartButton : MonoBehaviour{
	[SerializeField] private TextMeshProUGUI button;
	public void ButtonPress(){
		button.fontSize = 0f;
	}
}
