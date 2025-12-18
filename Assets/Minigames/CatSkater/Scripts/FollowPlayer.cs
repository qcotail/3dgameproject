using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour{
	public Transform victim;
	public int xOffset;
	public int yOffset;
	public int zOffset;

	void LateUpdate(){
		transform.position = victim.transform.position + new Vector3(xOffset, yOffset, zOffset);
	}
}
