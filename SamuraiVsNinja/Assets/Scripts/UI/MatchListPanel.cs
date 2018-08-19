using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class MatchListPanel : MonoBehaviour
{
	private void Awake ()
	{
		AvailableMatchesList.OnAvailableMatchesChanged += AvailableMatchesList_OnAvailableMatchesChanged;
	}

	private void AvailableMatchesList_OnAvailableMatchesChanged(List<MatchInfoSnapshot> matches)
	{
		ClearExistingButtons();
		CreateNewJoinGameButtons(matches);
	}

	private void ClearExistingButtons()
	{
		var buttons = GetComponentsInChildren<JoinMatchButton>();
		foreach (var button in buttons)
		{
			Destroy(button.gameObject);
		}
	}

	private void CreateNewJoinGameButtons(List<MatchInfoSnapshot> matches)
	{
		foreach (var match in matches)
		{
			var button = Instantiate(UIManager.Instance.MatchButtonPrefab);
			button.GetComponent<JoinMatchButton>().Initialize(match, transform);
			button.transform.SetParent(transform);
		}
	}
}
