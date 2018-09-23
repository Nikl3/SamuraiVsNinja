using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : UIPanel
{
    private Text wienerText;

    private void Awake()
    {
        wienerText = transform.Find("PlayerStatsPanel").transform.Find("WinnerText").GetComponent<Text>();
    }

    public override void OpenBehaviour()
    {
        base.OpenBehaviour();
        Time.timeScale = 0;

        var winnerName = LevelManager.Instance.WinnerName;
        wienerText.text = winnerName + "\nYou are THE winner!";
    }

    public override void CloseBehaviour()
    {
        base.CloseBehaviour();
    }

    public override void BackButton()
    {
        base.BackButton();

        GameMaster.Instance.LoadScene(0);
    }
}
