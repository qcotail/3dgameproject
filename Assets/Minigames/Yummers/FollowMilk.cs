using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMilk : MonoBehaviour
{
	public Transform victim;
	public int xOffset;
	public int yOffset;
	public int zOffset;
	public float minimumXComponent = 0f;
	public float maximumXComponent = 5f;
	public float minimumYComponent = 0f;
	public float maximumYComponent = 5f;
	public float zAxisUnchanged = 5f;

	void LateUpdate()
	{
		transform.position = victim.transform.position + new Vector3(xOffset, yOffset, zOffset);
	}

	void Start()
	{
		float rng1 = Random.Range(minimumXComponent, maximumXComponent);
		float rng2 = Random.Range(minimumYComponent, maximumYComponent);
		//victim.transform.position = new Vector3(rng1, rng2, zAxisUnchanged);
		transform.position = victim.transform.position + new Vector3(rng1, rng2, zOffset);
	}

}
