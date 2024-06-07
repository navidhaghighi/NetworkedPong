using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ClientBall : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 currentPosition;
    WebSocket ws;
    private SocketManager socketManager;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(SocketManager socketManager)
    {
        this.socketManager = socketManager;
        socketManager.onMessage += Ws_OnMessage;
    }

    private void Ws_OnMessage(string message)
    {
        Debug.LogWarning("Message received " + message);
        //ignore the message if it's the id of this paddle
        Message msg = JsonConvert.DeserializeObject<Message>(message);
        switch (msg.messageType)
        {
            case "Id":
                PaddleDetailsMessage idMsg = JsonConvert.DeserializeObject<PaddleDetailsMessage>(message);
                break;
            case "BallPositionMessage":
                BallPositionMessage updateMsg = JsonConvert.DeserializeObject<BallPositionMessage>(message);
                currentPosition = new Vector3(updateMsg.x,updateMsg.y, updateMsg.z);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        rb.position = currentPosition;   
    }
}
