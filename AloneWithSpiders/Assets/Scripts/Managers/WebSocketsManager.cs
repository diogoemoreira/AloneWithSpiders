using UnityEngine;
using TMPro;
using NativeWebSocket;
using System;
using System.IO;

public class WebSocketsManager : MonoBehaviour
{
    public static WebSocketsManager instance;

    public TextMeshProUGUI textComponent;

    public GameObject playerObject;

    private WebSocket websocket;

    private string filePath = "";

    private StreamWriter writer;

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
        //As the file name uses UTC time there is no problem with finding dupes
        filePath = Application.dataPath+"/Logs/LogFile_"+DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")+".txt";
        // Create the directory if it doesn't exist
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        // Create the file
        writer = new StreamWriter(filePath); //if we want to append use new StreamWriter(filePath, true)

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
            
            // Write to the file
            String spiderPos = "";
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Spider")){
                spiderPos += "["+go.transform.position.ToString()+"]";
            }
            writer.WriteLine($"Received Message: "+message+": PlayerPos: "+
                                playerObject.transform.position.ToString()+": SpidersPos: "+ spiderPos);
            
            //Debug.Log("Received Message: "+message+": PlayerPos: "+playerObject.transform.position.ToString()+": SpidersPos: "+ spiderPos);

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