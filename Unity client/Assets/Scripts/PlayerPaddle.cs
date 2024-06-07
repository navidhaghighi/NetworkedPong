using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class PlayerPaddle : MonoBehaviour
{
    private bool isOpen;
    private PaddleDirection direction;
    private float previousSentPosition;
    [SerializeField]
    private SocketManager socketManager;
    // Start is called before the first frame update
    void Start()
    {
        //RegisterPaddle();
    }

    public void Init(PaddleDirection paddleDirection,SocketManager socketManager)
    {
        this.direction = paddleDirection;
        this.socketManager = socketManager;
    }

    private void Ws_OnOpen(object sender, EventArgs e)
    {
        Debug.LogWarning("Opened");
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
       Debug.LogWarning("Message received " + e.Data);
        //ignore the message if it's the id of this paddle
        Message msg = JsonConvert.DeserializeObject<Message>(e.Data);
        switch (msg.messageType)
        {
            case "Id":
                PaddleDetailsMessage idMsg = JsonConvert.DeserializeObject<PaddleDetailsMessage>(e.Data);
                if (idMsg.hostOrClient == "Host")
                    this.direction = PaddleDirection.Left;
                else this.direction = PaddleDirection.Right;
                break;
            default:
                break;
        }
    }

    public void RegisterPaddle()
    {
      StartCoroutine(  socketManager.SendMessageViaSocket(JsonConvert.SerializeObject(new Message() { messageType = "RegisterPaddle" })));
    }


    private void Update()
    {
        if (transform.position.y != previousSentPosition)
        {
            previousSentPosition = transform.position.y;
            var json = JsonConvert.SerializeObject(new UpdatePosition() { messageType = "UpdatePosition", direction = this.direction, paddlePosition = previousSentPosition });
           StartCoroutine( socketManager.SendMessageViaSocket(json));
            Debug.Log("Sending position");
        }
    }
}
