using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkClientManager : NetworkBehaviour
{
    [SyncVar]
    private float timeLeft;
    private float startTime = 30f;
    private int restartCounter = 2;
    private bool isTimeRunning;
    [SerializeField]
	private GameObject playerPrefab;
	private string playerId;
	private NetworkIdentity networkIdentity;
    private NetworkClient networkClient;

    private void Start ()
	{
		networkIdentity = GetComponent<NetworkIdentity>();
		playerId = networkIdentity.netId.ToString();
		transform.name = "PlayerClient: " + playerId;
        StartCoroutine(ITimer());

        if (!hasAuthority)
			return;

		CmdSpawn(playerPrefab);
    }

    [Command]
	private void CmdSpawn(GameObject playerPrefab)
	{
		if (!isServer)
			return;

		var newPrefabInstance = Instantiate(playerPrefab);
		newPrefabInstance.name = playerPrefab.name;
		newPrefabInstance.transform.SetParent(transform);

		NetworkServer.SpawnWithClientAuthority(newPrefabInstance, connectionToClient);
	}

	private void Spawn(GameObject playerPrefab)
	{
		if (!isServer)
			return;

		var newPrefabInstance = Instantiate(playerPrefab);
		newPrefabInstance.name = playerPrefab.name;
		newPrefabInstance.transform.SetParent(transform);

		NetworkServer.SpawnWithClientAuthority(newPrefabInstance, connectionToClient);
	}

    private IEnumerator ITimer()
    {
        timeLeft = startTime;

        while (timeLeft > 0)
        {
            Mathf.RoundToInt(timeLeft -= Time.deltaTime);
            timeLeft = Mathf.Clamp(timeLeft, 0f, startTime);
            UIManager.Instance.ModifyTimerText(timeLeft.ToString("#"), Color.white);
            yield return null;
        }

        UIManager.Instance.ModifyTimerText("Time's up!", Color.white);

        yield return new WaitForSeconds(restartCounter);
    }
}