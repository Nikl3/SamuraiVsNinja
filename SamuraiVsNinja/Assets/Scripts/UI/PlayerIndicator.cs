using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
	private Text playerIdText;
	private Outline playerTextOutline;

	private void Awake ()
	{
		playerIdText = GetComponentInChildren<Text>();
		playerTextOutline = GetComponentInChildren<Outline>();
	}

	public void ChangeTextVisuals(string newText, Color textColor)
	{
		playerIdText.text = newText;
		playerIdText.color = textColor;
		playerTextOutline.effectColor = Color.black;
	}
}
