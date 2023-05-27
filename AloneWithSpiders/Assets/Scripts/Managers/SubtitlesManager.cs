using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitlesManager : MonoBehaviour
{
    public static SubtitlesManager instance;

    private TextMeshProUGUI textComponent;

    private Queue<string> subtittlesQueue;
    private bool canPlayNext = true;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }

    void Start()
    {
        textComponent = this.GetComponent<TextMeshProUGUI>();
        textComponent.enabled = false;
        subtittlesQueue = new Queue<string>();
    }

    public void DisplaySubtitles(string text)
    {
        if (!subtittlesQueue.Contains(text))
        {
            subtittlesQueue.Enqueue(text);
        }
    }

    private void Update()
    {
        if (canPlayNext && subtittlesQueue.Count > 0)
        {
            textComponent.text = subtittlesQueue.Dequeue();
            textComponent.enabled = true;
            canPlayNext = false;
            StartCoroutine(CountDownSubtitles());
        }
    }

    IEnumerator CountDownSubtitles()
    {
        yield return new WaitForSeconds(6);
        textComponent.enabled = false;
        canPlayNext = true;
    }
}
