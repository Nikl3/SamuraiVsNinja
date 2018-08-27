using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    private Image playerImage;
    private Text coinCountText;
    private string text;
    private int coins;

    private void Awake()
    {
        playerImage = GetComponent<Image>();
        coinCountText = GetComponentInChildren<Text>();
    }

    public void ModifyCoinValues(int amount)
    {
        coins += amount;
        coinCountText.text = coins.ToString();
    }
}
