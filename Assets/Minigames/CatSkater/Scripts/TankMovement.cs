using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TankMovement : MonoBehaviour{
	float x, z;                   /* PlayerInput        */
	float speed = 3f;            /* F/B Speed          */
	float turnSpeed = 0.2f;     /* L/R Speed          */
	Rigidbody playerRigidBody; /* Player             */

	void Start(){
		/* Assign Player to playerRigidBody */
		playerRigidBody = GetComponent<Rigidbody>();
	}

	void Update(){
		/* Get input values */
		x = Input.GetAxis("Horizontal"); /* F/B */
		z = Input.GetAxis("Vertical");  /* L/R */

		/* Going backwards will be slower */
		if (z < 0){
			z /= 6;
		}
	}

	private void FixedUpdate(){
		/* Tank Controls; Left/Right Rotate */
		Vector3 torque = Vector3.up * x * turnSpeed;
		playerRigidBody.AddTorque(torque);

		/* Tank Controls; Forward/Back is Relative to Angle */
		Vector3 velocity = transform.forward * z * speed;
		playerRigidBody.velocity = velocity;
	}
	void OnCollisionEnter(Collision c){
		/* If You Bump You Game Over */
		Debug.Log("Bump");
	}
}
