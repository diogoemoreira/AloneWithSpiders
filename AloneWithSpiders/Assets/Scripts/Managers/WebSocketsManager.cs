using UnityEngine;
using TMPro;
using NativeWebSocket;
using System;
using System.IO;
using System.Globalization;

public class WebSocketsManager : MonoBehaviour
{
    public static WebSocketsManager instance;

    //public TextMeshProUGUI textComponent;

    public GameObject playerObject;
    //public GameLogic gameLogic;

    private WebSocket websocket;

    private string filePath = "";

    private StreamWriter writer;

    double heartRate = 0;
    double heartRate1 = 0;
    bool first = true;

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
        //StreamWriter doesn't work with quest (WHY? Probably needs permission first)

        //As the file name uses UTC time there is no problem with finding dupes
        //filePath = Application.dataPath+"/Logs/LogFile_"+DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")+".txt";
        // Create the directory if it doesn't exist
        //Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        // Create the file
        //writer = new StreamWriter(filePath); //if we want to append use new StreamWriter(filePath, true)

        //textComponent.text = "Creating...";
        websocket = new WebSocket("ws://192.168.1.79:3000"); //the IP needs to be changed to your IPv4
        //textComponent.text = "Created";

        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

        //confirm if the connection was established correctly
        websocket.OnOpen += () => {
            Debug.Log("WebSocket is open!");
            //textComponent.text = "Connected!";
        };

        websocket.OnError += (e) => {
            Debug.Log("Error! " + e);
            //textComponent.text = "ERROR!";
        };

        websocket.OnClose += (e) => {
            Debug.Log("Websocket is closed!");
        };

        websocket.OnMessage += (bytes) =>   {
            // Reading a plain text message
            //Debug.Log("Bytes: "+bytes.Length+" the message in bytes: "+bytes);
            
            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            //Debug.Log("OnMessage! " + message);
            var vals = message.Split('\n');
            foreach(var val in vals){
                val.Replace('.',',');
                if(double.TryParse(val, out double aux)){
                    heartRate = aux;
                    if(first){
                        heartRate1 = aux;
                        first = false;
                    }
                }
            }
            
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

    async void SendWebSocketMessages()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            //await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            //await websocket.SendText("plain text message");
        }
    }

     public async void openConnection(){
        // waiting for messages
        await websocket.Connect();
    }

    public async void closeConnection(){
        CancelInvoke();
        await websocket.Close();
    }

    private void OnApplicationQuit()
    {
        websocket.Close();
    }
}
