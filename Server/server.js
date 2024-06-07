
const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 8080 });
let playersCount=0;
let gameID = 0;
let gameList = {};
var playersList = [];
var allConnections = [];
var maximumPlayersAllowed = 2;
wss.on('connection', (ws) => {
    allConnections.push(ws);  
    console.log('New connection');
    playersList.push(ws);
    playersCount++;
    console.log("Players count is "+ playersCount);
    if(playersCount==1)
    {
    console.log("Sending start host ");
    ws.send(JSON.stringify({
        messageType: "StartHost",
    }));
   } 
   else{
    ws.send(JSON.stringify({
        messageType: "StartClient",
    }));
   }
   if(playersCount == maximumPlayersAllowed)
    {
        console.log("reached maximum , now sendign startgame " );



        allConnections.forEach(function(item) {
            item.send(JSON.stringify({
                messageType: "StartGame",
            }));
        });
        
        

    }
    let hostOrClient = "";
    let direction = "Left";
    if(playersCount==1)
        {
        hostOrClient = "Host";
        direction = "Left";
        }
    else
    { 
        hostOrClient = "Client";
        direction = "Right";
    }
ws.send(JSON.stringify({
    messageType: "Id",
    "hostOrClient":hostOrClient,
    "direction": direction,
}));
    console.log("Registering paddle called ");
   // if(playersCount>=2)
       // JSON.parse(jsonString);

    ws.on('message', (message) => {
        console.log("receiveed this message "+ message);
        const deserializedMsg = JSON.parse(message);
        let x = message.x;
        switch(deserializedMsg.messageType) {
//send ball position to all connections
            case "BallPositionMessage":
                allConnections.forEach(function(item) {


                    item.send(JSON.stringify({
                        messageType: "BallPositionMessage",
                        x:deserializedMsg.x,
                        y:deserializedMsg.y,
                        z:deserializedMsg.z,
                    }));
                    console.log("sending this message "+ deserializedMsg);
                });
                break;
            case "UpdatePosition":
                allConnections.forEach(function(item) {
                    item.send(JSON.stringify({
                        messageType: "UpdatePosition",
                        direction: deserializedMsg.direction,
                        paddlePosition: deserializedMsg.paddlePosition,
                    }));
                    console.log("sending this message "+ message);
                });
              break;
            case "RegisterPaddle":
       
              break;
            default:
              break;
          }

        onPaddleMoved();
    });

    ws.on('close', () => {
        delete gameList[ws.id];
    });
});

function onPaddleMoved()
{

}
console.log('Server running at http://127.0.0.1:8080/');

