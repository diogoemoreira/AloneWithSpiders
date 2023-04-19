using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using NativeWebSocket;

public class WebSocketManager : MonoBehaviour
{
    WebSocket websocket;

    // Start is called before the first frame update
    void Start()
    {
        websocket = new WebSocket("ws://localhost:3000");

        websocket.OnOpen += () => {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) => {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) => {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) => {
            Debug.Log("OnMessage!");
            Debug.Log(bytes);

            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
        };
    }

    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
        #endif
    }

    async void SendWebSocketMessages()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText("plain text message");
        }
    }

    public async void sendMessage(TMP_InputField sendText){
        if (websocket.State != WebSocketState.Open){
            // waiting for messages
            await websocket.Connect();
        }
        if (websocket.State == WebSocketState.Open)
        {
            string text2Send = "empty message";
            Debug.Log(sendText.text);
            if(sendText.text !="")
                text2Send=sendText.text;

            // Sending bytes
            await websocket.SendText(text2Send);
        }
    }

    public void startTestMessages(){
        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessages", 0.0f, 0.3f);        
    }

    public async void openConnection(){
        // waiting for messages
        await websocket.Connect();
    }

    public async void closeConnection(){
        CancelInvoke();
        await websocket.Close();
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
