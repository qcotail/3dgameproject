using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class TEMP_TextManager : MonoBehaviour{
	public InputActionReference newInput;

	private float SpacebarInput;
	private string CurrentSceneName;
	private string NextSceneName;
	static private TextMeshProUGUI UIText;
	static private Dictionary<string, string> sceneDestination;

	void Awake(){
		sceneDestination = new Dictionary<string ,string>(){
			{"kurt_scene", "kurt_scene_2"},
			{"kurt_scene_2", "kurt_scene"},
		};
	}

	void Start(){
		UIText = GetComponent<TextMeshProUGUI>();
		CurrentSceneName = "";
		NextSceneName = "";
	}

	void Update(){
		CurrentSceneName = SceneManager.GetActiveScene().name;
		NextSceneName = sceneDestination[CurrentSceneName];
		/*
		if (CurrentSceneName == "kurt_scene"){
			NextSceneName = "kurt_scene_2";
		}
		if (CurrentSceneName == "kurt_scene_2") {
			NextSceneName = "kurt_scene";
		}
		*/

		SpacebarInput = newInput.action.ReadValue<float>();
		UIText.text = "Press Spacebar To Change Scene";
		if (SpacebarInput != 0){
			//UIText.text = "Spacebar Pressed!";
			SceneManager.LoadScene(NextSceneName);
		}
	}
}
