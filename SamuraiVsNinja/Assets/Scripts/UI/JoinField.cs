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

    private readonly string defaultText = "JOIN";

    public Color CurrentColor
    {
        get;
        private set;
    }

    private void Awake()
    {
        joinedPlayerName = GetComponentInChildren<Text>();
        fieldImage = GetComponent<Image>();
        icon = fieldImage.transform.GetChild(1).GetComponent<Image>();
        defaultColor = fieldImage.color;
    }

    public void ChangeSprite(Color spriteColor)
    {
        CurrentColor = icon.color = spriteColor;
    }

    public void ChangeJoinFieldVisuals(int playerID, Color fieldColor)
    {
        joinedPlayerName.text = "Player " + playerID;
        fieldImage.color = fieldColor;
        HasJoined = true;
    }

    public void UnChangeJoinFieldVisuals()
    {
        joinedPlayerName.text = defaultText;
        fieldImage.color = defaultColor;
        HasJoined = false;
        icon.color = defaultColor;
    }
}
