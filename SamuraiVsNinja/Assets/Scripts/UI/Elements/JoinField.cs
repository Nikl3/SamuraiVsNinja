using UnityEngine;
using UnityEngine.UI;

public class JoinField : MonoBehaviour
{
    private Text joinedPlayerName;
    private Image fieldImage;
    private Image icon;
    private Color defaultColor;

    public bool HasJoined
    {
        get;
        private set;
    }

    private readonly string defaultText = "Press START to join";

    private void Awake()
    {
        joinedPlayerName = GetComponentInChildren<Text>();
        fieldImage = GetComponent<Image>();
        icon = fieldImage.transform.GetChild(1).GetComponent<Image>();
        icon.gameObject.SetActive(false);
        defaultColor = fieldImage.color;
    }

    public void ChangeSprite(Sprite newIconSprite)
    {       
        icon.sprite = newIconSprite;
    }

    public void ChangeJoinFieldVisuals(int playerID, Color fieldColor)
    {
        joinedPlayerName.text = "PLAYER " + playerID;
        fieldImage.color = fieldColor;
        HasJoined = true;
        icon.gameObject.SetActive(true);
    }

    public void UnChangeJoinFieldVisuals()
    {
        joinedPlayerName.text = defaultText;
        fieldImage.color = defaultColor;
        HasJoined = false;
        icon.gameObject.SetActive(false);
    }
}
