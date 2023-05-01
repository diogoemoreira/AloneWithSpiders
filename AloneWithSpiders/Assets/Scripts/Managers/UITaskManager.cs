using TMPro;
using UnityEngine;

public class UITaskManager : MonoBehaviour
{
    public static UITaskManager instance;
    [SerializeField]
    private TextMeshProUGUI text;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }
    
    public void SetCurrentTask(string txt)
    {
        text.text = txt;
    }
}
