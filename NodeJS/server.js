var websocket = require('ws');

var callbackInitServer = () => {
    console.log("Server is running.");
}
var wss = new websocket.Server({ port: 65000 }, callbackInitServer)

var wslist = [];

wss.on("connection", (ws) => {
    //console.log("client connected.");
    wslist.push(ws);

    for (var i = 0; i < wslist.length ; i++)
    {
        if (wslist[i] == ws)
        {
            console.log("Client " + i + " is connected.");
            continue;
        }
    }

    ws.on("message", (data) => {

        for (var i = 0; i < wslist.length; i++) {

            if (wslist[i] != ws) {
                wslist[i].send(data + "                                                     " );
                continue;
            }

            if (wslist[i] == ws) {
                console.log("Client " + i + " send : " + data);
                wslist[i].send(data);
                continue;
            }

           
        }
        //console.log("send from client :" + data);
    });

    ws.on("close", () => {
        //console.log("client disconnected.");
        
        for (var i = 0; i < wslist.length; i++) {
            if (wslist[i] == ws) {
                console.log("Client " + i + " is disconnected.");
                wslist = ArrayRemove(wslist, ws);
            }
        }
    });
});

function ArrayRemove(arr, value)
{
    return arr.filter((element) => {
        return element != value;
    })
}

function Boardcast(data)
{
    for (var i = 0; i < wslist.length; i++)
    {
        wslist[i].send(data); 
    }
}
