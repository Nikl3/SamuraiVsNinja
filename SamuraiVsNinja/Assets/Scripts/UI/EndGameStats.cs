using UnityEngine;
using UnityEngine.UI;

public class EndGameStats : MonoBehaviour
{
    public Image PlayerIcon;
    public Text PlayerName;
    public Text OnigirisPicked;
    public Text OnigirisLost;
    public Text Kills;
    public Text Deaths;
    public Text HitPercent;

    private void OnEnable()
    {
        transform.SetParent(UIManager_Old.Instance.PlayerEndPanel);
        transform.localScale = Vector2.one;
    }

    public void SetEndGameStats(Sprite playerIconSprite, string playerName, int onigirispicked, int onigirislost, int kills, int deaths, float hitPercent) 
    {
        PlayerIcon.sprite = playerIconSprite;
        PlayerName.text = playerName;
        OnigirisPicked.text = "Onigiris picked: " + onigirispicked;
        OnigirisLost.text = "Onigiris lost: " + onigirislost;
        Kills.text = "Players killed: " + kills;
        Deaths.text = "Deaths: " + deaths;
        HitPercent.text = "Hit %: " + hitPercent.ToString("F1") +"%";
    }
}
