using UnityEngine;
using UnityEngine.UI;

public class EndGameStats : MonoBehaviour
{
    [SerializeField]
    private Image PlayerIcon;
    [SerializeField]
    private Text PlayerName;
    [SerializeField]
    private Text OnigirisPicked;
    [SerializeField]
    private Text OnigirisLost;
    [SerializeField]
    private Text Kills;
    [SerializeField]
    private Text Deaths;
    [SerializeField]
    private Text HitPercent;

    private void OnEnable()
    {
        transform.SetParent(UIManager.Instance.PlayerEndPanel);
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
