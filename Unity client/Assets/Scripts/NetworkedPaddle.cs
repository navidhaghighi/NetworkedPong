using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
public class NetworkedPaddle : PaddleController
{
    private float currentPosition;
    private Rigidbody2D rb2d;
    private bool isOpen;
    private PaddleDirection direction;
    private float previousSentPosition;
    [SerializeField]
    private SocketManager socketManager;


    public void Init(PaddleDirection direction, SocketManager socketManager)
    {
        this.direction = direction;
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
            case "UpdatePosition":
                UpdatePosition updateMsg = JsonConvert.DeserializeObject<UpdatePosition>(message);
                if (updateMsg.direction == direction)
                    currentPosition = updateMsg.paddlePosition;
                break;
            default:
                break;
        }


    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, currentPosition, transform.position.z);
    }
}
public enum PaddleDirection
{
    Right,
    Left
}
[Serializable]
public class Message
{
    public string messageType;
}

[System.Serializable]
public class PaddleDetailsMessage:Message
{
    public string hostOrClient;
    public PaddleDirection direction;
}

[System.Serializable]
public class UpdatePosition:Message
{
    public PaddleDirection direction;
    public float paddlePosition;
}
[System.Serializable]
public class BallPositionMessage:Message
{
    public float x;
    public float y;
    public float z;
}
