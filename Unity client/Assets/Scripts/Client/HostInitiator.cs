using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class HostInitiator : MonoBehaviour
{
    [SerializeField]
    private GameObject hostPaddle;
    [SerializeField]
    private GameObject clientPaddle;
    private bool startedGame;
    private bool gameStartMessageReceived;
    [SerializeField]
    private SocketManager _socketManager;
    private bool isHost;
    [SerializeField]
    private GameObject ballReference;
    void Start()
    {
        _socketManager.onMessage += Ws_OnMessage;

    }

    private void Ws_OnOpen(object sender, EventArgs e)
    {
        Debug.LogWarning("Opened");
    }

    private void Ws_OnMessage(string message)
    {
        Debug.LogWarning("Message received " + message);
        //ignore the message if it's the id of this paddle
        Message msg = JsonConvert.DeserializeObject<Message>(message);
        switch (msg.messageType)
        {
            case "StartHost":
                StartHost();
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
        if (isHost)
        {
            ballReference.AddComponent<HostBall>();
            var hostBall = ballReference.GetComponent<HostBall>();
            hostBall.Init(_socketManager);
            hostPaddle.AddComponent<PlayerControls>();
            var hostPaddleComponent =  hostPaddle.AddComponent<PlayerPaddle>();
            hostPaddleComponent.Init( PaddleDirection.Left,_socketManager);
            var clientPaddleComponent =  clientPaddle.AddComponent<NetworkedPaddle>();
            clientPaddleComponent.Init ( PaddleDirection.Right, _socketManager);
        }
    }


    public void StartHost()
    {
        isHost = true;
    }

    private void Update()
    {
        if (gameStartMessageReceived && !startedGame)
            OnGameStarted();
    }
}
