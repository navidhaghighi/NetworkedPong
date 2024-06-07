using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class WaitingFor2Players : MonoBehaviour
{
    private bool allPlayersConnected;
    private bool isHost;
    //this message will be received when all of the required players are connected.
    private const string requiredPlayersConnectedMsg = "StartGame";
    [SerializeField]
    //this gameobject will block UI until all required players are connected;
    private GameObject blocker;
    [SerializeField]
    private TextMeshProUGUI label;
    [SerializeField]
    private SocketManager socketManager;
    // Start is called before the first frame update
    void Start()
    {
        socketManager.onMessage+= Ws_OnMessage;
    }



    private void Ws_OnMessage(string message)
    {
        var deserializedMsg = JsonConvert.DeserializeObject<Message>(message);
        switch (deserializedMsg.messageType)
        {
            case requiredPlayersConnectedMsg:
                allPlayersConnected = true;
                break;
            case "StartHost":
                isHost = true;
                break;
            default:
                break;
        }

    }


    private void Update()
    {
        if(isHost)
            label.text = "You are the host , wait for the other client to connect ";
        if(allPlayersConnected)
            blocker.SetActive(false);
    }
}
