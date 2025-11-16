using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem

public class TEMP_ChangeScene : MonoBehaviour{
	public InputActionReference newInput;
	void Start(){
	}

	void Update(){
		Debug.Log(newInput.action);
	}
}
