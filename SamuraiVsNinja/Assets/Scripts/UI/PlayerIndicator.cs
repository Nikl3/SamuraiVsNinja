using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
	private Text playerIdText;

	public string PlayerIdText
	{
		get
		{
			return playerIdText.text;
		}
		set
		{
			playerIdText.text = value;
		}
	}

	private void Awake ()
	{
		playerIdText = GetComponentInChildren<Text>();
	}
}
