using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostBall : MonoBehaviour {
	private bool gameStarted;
	private Rigidbody2D rb2d;
	private SocketManager socketManager;
	void GoBall() {
		float rand = Random.Range (0, 2);
		if (rand < 1) {
			rb2d.AddForce (new Vector2 (20, -15));
		} else {
			rb2d.AddForce (new Vector2 (-20, -15));
		}
	}


	public void Init(SocketManager socketManager)
	{
		this.socketManager = socketManager;
        socketManager.onMessage += OnMessage;
    }

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		Invoke ("GoBall", 2);

		
	}

	private void OnMessage(string message)
	{
		switch (message)
		{
			case "StartGame":
				gameStarted = true;
				break;
			default:
				break;
		}
	}

	void ResetBall() {
		rb2d.velocity = new Vector2 (0, 0);
		transform.position = Vector2.zero;
	}

	void RestartGame() {
		ResetBall ();
		Invoke ("GoBall", 1);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.CompareTag ("Player")) {
			Vector2 vel;
			vel.x = rb2d.velocity.x;
			vel.y = (rb2d.velocity.y / 2.0f) + (coll.collider.attachedRigidbody.velocity.y / 3.0f);
			rb2d.velocity = vel;
		}
	}

	private void Update()
	{
		StartCoroutine( socketManager.SendMessageViaSocket(JsonConvert.SerializeObject(new BallPositionMessage() { messageType = "BallPositionMessage", x = transform.position.x, y = transform.position.y, z = transform.position.z })));
	}

}
