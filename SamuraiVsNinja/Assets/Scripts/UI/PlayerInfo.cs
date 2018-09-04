using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    private Transform parentContainer;
    //private Image playerImage;
    private Text coinCountText;
    private string text;
    private int coins;

    private void Awake()
    {
        //playerImage = GetComponent<Image>();
        coinCountText = GetComponentInChildren<Text>();
        parentContainer = GameObject.Find("HUD").transform.Find("PlayerInfoContainer");
    }

    private void Start()
    {
        transform.SetParent(parentContainer);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public void ModifyCoinValues(int amount)
    {
        coins += amount;
        coinCountText.text = coins.ToString();
    }
}
