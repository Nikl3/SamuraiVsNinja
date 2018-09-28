using UnityEngine;
using UnityEngine.UI;

public class EndGameStats : MonoBehaviour {

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
    private Text Attacks;
    [SerializeField]
    private Text HitPerc;

    public void SetEGStats(string playerName, int onigirispicked, int onigirislost, int kills, int deaths, int attacks, int hitperc) 
    {
        PlayerName.text = playerName;
        OnigirisPicked.text = "Onigiris picked: " + onigirispicked;
        OnigirisLost.text = "Onigiris lost: " + onigirislost;
        Kills.text = "Players killed: " + kills;
        Deaths.text = "Deaths: " + deaths;
        Attacks.text = "Attacks: " + attacks;
        HitPerc.text = "Hit % " + hitperc;

    }
}
