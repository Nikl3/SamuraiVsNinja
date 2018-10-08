using UnityEngine;
using UnityEngine.UI;

public class JoinField : MonoBehaviour
{
    private Text joinedPlayerName;
    private Image fieldImage;
    private Image icon;
    private Color defaultColor;
    private GameObject AddPlayerText;

    public bool HasJoined
    {
        get;
        private set;
    }

    private void Awake()
    {
        joinedPlayerName = GetComponentInChildren<Text>();
        AddPlayerText = transform.Find("AddPlayerText").gameObject;
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
        joinedPlayerName.gameObject.SetActive(true);
        joinedPlayerName.text = "PLAYER " + playerID;
        fieldImage.color = fieldColor;
        HasJoined = true;
        icon.gameObject.SetActive(true);
        AddPlayerText.SetActive(false);
    }

    public void UnChangeJoinFieldVisuals()
    {
        joinedPlayerName.gameObject.SetActive(false);
        fieldImage.color = defaultColor;
        HasJoined = false;
        icon.gameObject.SetActive(false);
        AddPlayerText.SetActive(true);
    }
}
