using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : UIPanel
{
    private Text wienerText;

    private void Awake()
    {
        wienerText = transform.Find("WinnerText").GetComponent<Text>();
    }

    public override void OpenBehaviour()
    {
        base.OpenBehaviour();

        //wienerText.text = winnerName + "\nYou are THE winner!";
        Time.timeScale = 0;
    }

    public override void CloseBehaviour()
    {
        base.CloseBehaviour();
    }
}
