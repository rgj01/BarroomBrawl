/*global 
 us: true,
 require: true,
 setInterval: true,
*/

var net = require('net')

var HOST = '0.0.0.0'
var PORT = 8084
var MAXTABLES = 50
var MAX_X = 1000
var MAX_Y = 1000
var clients = []
var gameObjects = generateGameState()

// Broadcast game state updates every 50 ms
// setInterval(function () {
//   if (clients.length) {
//     // client
//     clients.forEach(function (client) {
//       client.write(JSON.stringify({"messageType": "update", "gameState": gameObjects}))
//     })
//   }
// }, 2000)

// Create a server instance, and chain the listen function to it
// The function passed to net.createServer() becomes the event handler for the 'connection' event
// The socket object the callback function receives UNIQUE for each connection
net.createServer(function (socket) {

    // We have a connection - a socket object is assigned to the connection automatically
    console.log('CLIENT JOINED: ' + socket.remoteAddress +':'+ socket.remotePort)
    console.log("Adding new player")
    
    var clientName = socket.remoteAddress + ":" + socket.remotePort
    
    clients.push(socket)
    
    console.log("Current " + clients.length + " Clients: " + clients.toString())
    
    // Add a new player
    var newPlayer = generatePlayer()
    gameObjects.push(newPlayer)
    
    var buffer = ""
    
    // Add a 'data' event handler to this instance of socket
    socket.on('data', function (data) {
      
      buffer += data.toString()

      while (buffer.indexOf("END") !== -1) {
        message = buffer.slice(0, buffer.indexOf("END"))
        console.log("\n\n" + message)
        buffer = buffer.substring(buffer.indexOf("END") + 4, buffer.length)
        console.log("\n\nRemaining buffer: \n\n")
        process(message)
      }
      
      function process(message) {
        try {
          // console.log("\n\nDATAZ (SIZE " + data.toString().length + "): " + data.toString() + "\n\n")
          message = JSON.parse(message)
        } catch (e) {
          try {
            message = JSON.parse("{" + message)
          } catch (err) {
            console.log("WTF")
          }
          return
        }
        

        if (message.messageType === 'Join') {
          console.log("sending" + ((clients.length === 1) ? " - SERVER" : ""))
          console.log(socket.toString())
          
          socket.write(JSON.stringify({
            messageType: "JoinResponse",
            "gameState": gameObjects,
            "PlayerState": newPlayer,
            "Server": (clients.length === 1)
          }))
        } else if (message.messageType === "ServerUpdate") {
          gameObjects = message.gameState
        } else if (message.messageType === "PlayerUpdate") {
          clients[0].write(JSON.stringify(data))
        }
        
      }
      
    })
    
    // Add a 'close' event handler to this instance of socket
    socket.on('close', function (data) {
      // clients.splice(clients.indexOf(clientName), 1)
      console.log('CLIENT LEFT: ' + socket.remoteAddress +' '+ socket.remotePort)
    })
    
}).listen(PORT, HOST)

console.log('Server listening on ' + HOST +':'+ PORT)


// Helper functions

function generateGameState() {
  var numTables = Math.floor(Math.random() * MAXTABLES)
  console.log()
  
  var gameObjects = []
  
  for (var i = 0; i < numTables; i++) {
    gameObjects.push({
      type: "object",
      "m_textureName": "Table",
      Location: ((Math.floor(Math.random() * MAX_X)) + ", " + (Math.floor(Math.random() * MAX_Y))),
      Bounds: "32, 64",
      speed: 0,
      Direction: "None",
      Mobile: false,
      Solid: true
    })
  }
  
  return gameObjects;
}


var playerid = 0;

function generatePlayer() {
  return {
    type: "player",
    "m_textureName": "Player",
    Location: ((Math.floor(Math.random() * MAX_X)) + ", " + (Math.floor(Math.random() * MAX_Y))),
    Bounds: "64, 128",
    speed: 0,
    Direction: "None",
    Mobile: true,
    Solid: true,
    Playing: true,
    Health: 100,
    Drunkenness: 1,
    CarriedItemId: 0,
    ServerAssignedId: (++playerid),
    drunkennessClock: 0,
    invtimer: 0
  }
}




















