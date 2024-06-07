using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ClientInitiator : MonoBehaviour
{
    [SerializeField]
    private GameObject clientPaddle;
    [SerializeField]
    private GameObject hostPaddle;
    private bool startedGame;
    private bool gameStartMessageReceived;
    private bool isClient;
    [SerializeField]
    private SocketManager socketManager;
    [SerializeField]
    private GameObject ballGameObject;
    void Start()
    {
        socketManager.onMessage += Ws_OnMessage;

    }

    private void Ws_OnMessage(string message)
    {
        Debug.LogWarning("Message received " + message);
        //ignore the message if it's the id of this paddle
        Message msg = JsonConvert.DeserializeObject<Message>(message);
        switch (msg.messageType)
        {
            case "StartClient":
                StartClient();
                break;
            case "StartGame":
                gameStartMessageReceived = true;
                break;
            default:
                break;
        }
    }

    public void OnGameStarted()
    {
        startedGame = true;
        if (isClient)
        {
            var clientBall =  ballGameObject.AddComponent<ClientBall>();
            clientBall.Init(socketManager);
            var clientPaddleComponent =  clientPaddle.AddComponent<PlayerPaddle>();
            clientPaddle.AddComponent<PlayerControls>();
            clientPaddleComponent.Init( PaddleDirection.Right,socketManager);
            var hostPaddleComponent =  hostPaddle.AddComponent<NetworkedPaddle>();
            hostPaddleComponent.Init( PaddleDirection.Left, socketManager);
        }

    }


    public void StartClient()
    {
        isClient = true;
    }

    private void Update()
    {
        if (gameStartMessageReceived && !startedGame)
            OnGameStarted();
    }
}
