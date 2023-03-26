using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Confluent.Kafka;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;

public class KafkaManager : MonoBehaviour
{
    public static KafkaManager instance;


    private string brokerList = "localhost:9092";
    private string groupId = "sgarcia";
    private IConsumer<Ignore, string> consumer;
    private string topic="purchases";

    //the name of the file should be dynamic
    private string filePath = "";

    // The queue where we'll store the consumed messages
    private ConcurrentQueue<string> messageQueue;

    // The thread where the consumer will run
    private Thread consumerThread;
    private StreamWriter writer;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;     

        //As the file name uses UTC time there is no problem with finding dupes
        filePath = Application.dataPath+"/Logs/LogFile_"+DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")+".txt";
        // Create the directory if it doesn't exist
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        // Create the file
        writer = new StreamWriter(filePath); //if we want to append use new StreamWriter(filePath, true)
        
        Debug.Log(Application.dataPath);

        //Kafka consumer
        var config = new ConsumerConfig
        {
            BootstrapServers = brokerList,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        this.consumer = new ConsumerBuilder<Ignore, string>(config)
                            .SetErrorHandler((_, e) => Debug.LogError($"Error: {e.Reason}"))
                            .Build();
        //

        messageQueue = new ConcurrentQueue<string>();

        consumerThread = new Thread(new ThreadStart(StartConsuming));
        consumerThread.Start();

    }

    // Update is called once per frame
    void Update()
    {
        // Check if there are any messages in the queue
        if(!messageQueue.IsEmpty){
            while (messageQueue.TryDequeue(out string message))
            {
                // Do something with the message (e.g. update the UI)
                Debug.Log("Received message: " + message);
            }
        }
    }

    public void StartConsuming()
    {
        Debug.Log("Started");
        this.consumer.Subscribe(this.topic);

        while (true)
        {
            try
            {
                var consumeResult = this.consumer.Consume();
                messageQueue.Enqueue(consumeResult.Message.Value);
                Debug.Log($"Received message: {consumeResult.Message.Value}");
                
                // Write to the file
                writer.WriteLine($"Received message: {consumeResult.Message.Value}");
            }
            catch (ConsumeException e)
            {
                Debug.LogError($"Error occurred while consuming message: {e.Error.Reason}");
            }
        }
    }

    // Stop the consumer thread when the game object is destroyed
    private void OnDestroy()
    {
        //game crashes if uncommented
        //this.consumer.Close();
        writer.Close();
        consumerThread.Abort();
    }
}

