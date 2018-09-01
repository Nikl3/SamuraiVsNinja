using UnityEngine;
using UnityEngine.UI;

public class JoinField : MonoBehaviour
{
    private Text joinedPlayerName;
    private Image fieldImage;
    private Image icon;
    private Color defaulcolor;
    private string defaultText = "JOIN";

    private void Awake()
    {
        joinedPlayerName = GetComponentInChildren<Text>();
        fieldImage = GetComponent<Image>();
        icon = fieldImage.GetComponentInChildren<Image>();
        defaulcolor = fieldImage.color;
    }

    public void ChangeJoinFieldVisuals(int playerID, Color fieldColor)
    {
        joinedPlayerName.text = "Player " + playerID;
        fieldImage.color = fieldColor;
    }

    public void UnChangeJoinFieldVisuals()
    {
        joinedPlayerName.text = defaultText;
        fieldImage.color = defaulcolor;
    }
}
