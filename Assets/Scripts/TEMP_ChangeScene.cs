using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TEMP_ChangeScene : MonoBehaviour{
	public InputActionReference newInput;

	private float SpacebarInput;
	static private TextMeshProUGUI UIText;

	void Start(){
		UIText = GetComponent<TextMeshProUGUI>();
	}

	void Update(){
		SpacebarInput = newInput.action.ReadValue<float>();

		if (SpacebarInput != 0){
			Debug.Log(SpacebarInput);
			UIText.text = "Spacebar is being pressed.";
		}
		else {
			UIText.text = "AIUEO";
		}
	}
}
