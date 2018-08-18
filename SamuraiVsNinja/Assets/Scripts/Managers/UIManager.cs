using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
    [SerializeField]
    private Text networkDebugText;

    //[SerializeField]
    //private Text latencyDebugText;

    private void Start()
    {
        UpdateNetwprkText("Network text");
    }

    public void UpdateNetwprkText(string newText)
    {
        networkDebugText.text = newText;
    }

    //public void UpdateLatencyText(string newText)
    //{
    //    latencyDebugText.text = "Latency: " + newText;
    //}
}
