using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TankMovement : MonoBehaviour{
	float x, z;                   /* PlayerInput        */
	float speed = 20f;           /* F/B Speed          */
	float turnSpeed = 50f;     /* L/R Speed          */
	Rigidbody playerRigidBody; /* Player             */

	private float startY;

	[SerializeField] LevelTemplate lvltmp;

	void Start(){
		/* Assign Player to playerRigidBody */
		playerRigidBody = GetComponent<Rigidbody>();
		startY = playerRigidBody.position.y;
	}

	void Update(){
		/* Get input values */
		if (lvltmp.CanPlay()){
			x = Input.GetAxis("Horizontal"); /* L/R */
			z = Input.GetAxis("Vertical");  /* F/B */
		}
	}

	private void FixedUpdate(){
		/* Tank Controls; Left/Right Rotate */
		float turn = x * turnSpeed * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
		playerRigidBody.MoveRotation(playerRigidBody.rotation * turnRotation);

		/* Tank Controls; Forward/Back is Relative to Angle */
		Vector3 velocity = transform.forward * z * speed;
		playerRigidBody.velocity = velocity;

		float curY = playerRigidBody.position.y;
		if (curY != startY){
			lvltmp.FinishMinigame(false);
			playerRigidBody.useGravity = true;
		}
	}
	void OnCollisionEnter(Collision c){
		string cName = c.gameObject.name;

		if (cName == "Success"){
			lvltmp.FinishMinigame(true);
		}
	}
}
