using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class TEMP_TextManager : MonoBehaviour{
	static private TextMeshProUGUI UIText;

	/* Input Management */
	public InputActionReference newInput;
	private float SpacebarInput;

	/* Scene Management */
	static private Dictionary<string, string> sceneDestination;
	private string CurrentSceneName = "";
	private string NextSceneName = "";

	void Awake(){
		sceneDestination = new Dictionary<string ,string>(){ /* Origin Scene -> Destination Scene */
			{"kurt_scene", "kurt_scene_2"},
			{"kurt_scene_2", "kurt_scene"},
		};
	}

	void Start(){
		UIText = GetComponent<TextMeshProUGUI>();
	}

	void Update(){
		UIText.text = "Press Spacebar To Change Scene";

		/* Get Current and Next Scene */
		CurrentSceneName = SceneManager.GetActiveScene().name;
		NextSceneName = sceneDestination[CurrentSceneName];

		/* Read Spacebar Input */
		SpacebarInput = newInput.action.ReadValue<float>();
		if (SpacebarInput != 0){
			SceneManager.LoadScene(NextSceneName);
		}
	}
}
