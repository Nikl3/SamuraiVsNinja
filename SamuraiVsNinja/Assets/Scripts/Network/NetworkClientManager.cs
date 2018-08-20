using UnityEngine;
using UnityEngine.Networking;

public class NetworkClientManager : NetworkBehaviour
{
    [SyncVar]
    float timeLeft = 10f;
    [SerializeField]
	private GameObject playerPrefab;
	private string playerId;
	private NetworkIdentity networkIdentity;

	private void Start ()
	{
		networkIdentity = GetComponent<NetworkIdentity>();
		playerId = networkIdentity.netId.ToString();
		transform.name = "PlayerClient: " + playerId;

		if (!hasAuthority)
			return;

		CmdSpawn(playerPrefab);
	}

    private void Update()
    {
        if(timeLeft <= 0)
        {
            timeLeft = 0;
            UIManager.Instance.TimerText.color = Color.white;
            UIManager.Instance.TimerText.text = 0.ToString();
        }
        else if (timeLeft <= 20 && timeLeft > 10)
        {
            UIManager.Instance.TimerText.color = Color.yellow;
            UIManager.Instance.TimerText.text = timeLeft.ToString("#");
            timeLeft -= Time.deltaTime;
        }
        else if (timeLeft <= 10)
        {
            UIManager.Instance.TimerText.color = Color.red;
            UIManager.Instance.TimerText.text = timeLeft.ToString("#");
            timeLeft -= Time.deltaTime;
        }

        
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
}