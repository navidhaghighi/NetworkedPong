using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalls : MonoBehaviour {

	[SerializeField]
	private GameManager gameManager;
	void OnTriggerEnter2D(Collider2D hitInfo) {
		if (hitInfo.name == "Ball")
		{
			string wallName = transform.name;
			gameManager.Score (wallName);
			hitInfo.gameObject.SendMessage ("RestartGame", 1, SendMessageOptions.RequireReceiver);
		}
	}
}
