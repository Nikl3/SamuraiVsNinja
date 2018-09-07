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
        icon = fieldImage.transform.GetChild(1).GetComponent<Image>();
        defaulcolor = fieldImage.color;
    }

    public void ChangeSprite(Color spriteColor)
    {
        icon.color = spriteColor;
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
