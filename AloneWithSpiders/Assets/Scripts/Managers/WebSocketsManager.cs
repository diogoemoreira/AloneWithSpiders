using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using NativeWebSocket;

public class WebSocketsManager : MonoBehaviour
{
    public static WebSocketsManager instance;

    public TextMeshProUGUI textComponent;

    private WebSocket websocket;

    void Awake(){
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket("ws://localhost:3000");

        //confirm if the connection was established correctly
        websocket.OnOpen += () => {
            Debug.Log("WebSocket is open!");
        };

        websocket.OnError += (e) => {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) => {
            Debug.Log("Websocket is closed!");
        };

        websocket.OnMessage += (bytes) =>   {
            // Reading a plain text message
            //Debug.Log("Bytes: "+bytes.Length+" the message in bytes: "+bytes);
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("RECEIVED MESSAGE: "+message);
            textComponent.text = message;
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessages", 0.0f, 0.3f);   

        await websocket.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
        #endif
    }

    private void OnApplicationQuit()
    {
        websocket.Close();
    }
}
