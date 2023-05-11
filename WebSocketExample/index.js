const express = require('express');
const { createServer } = require('http');
const WebSocket = require('ws');
const { spawn } = require('child_process');

const app = express();
const port = 3000;

const server = createServer(app);
const wss = new WebSocket.Server({ server });

wss.on('connection', function(ws) {
  console.log("client joined.");

  // send "hello world" interval
  //const textInterval = setInterval(() => ws.send("hello world!"), 100);

  // send random bytes interval
  //const binaryInterval = setInterval(() => ws.send(crypto.randomBytes(8).buffer), 110);

  // Spawn a Python process to get data from your PLUX device
  const pythonProcess = spawn('python3', ['data.py']);

  // Handle output from the Python process
  pythonProcess.stdout.on('data', (data) => {
  //console.log("data received from Python process:", data.toString ('utf-8'));

    // Send the data to the client
    ws.send(data.toString ('utf-8'));
    var utcStr = new Date().toUTCString();
    //ws.send(utcStr);
    const now = new Date().toISOString();
    //ws.send(now);
  });

  ws.on('message', function(data) {
    if (typeof(data) === "string") {
      // client sent a string
      console.log("string received from client -> '" + data + "'");

    } else {
      console.log("binary received from client -> " + Array.from(data).join(", ") + "");
    }
  });

  ws.on('close', function() {
    console.log("client left.");
    //clearInterval(textInterval);
    //clearInterval(binaryInterval);
    pythonProcess.kill();
  });
});

server.listen(port, function() {
  console.log(`Listening on http://localhost:${port}`);
});