using UnityEngine;
using UnityEngine.Networking;

public class NetworkClientManager : NetworkBehaviour
{
	[SerializeField]
	private GameObject playerPrefab;
	private string playerId;
	private NetworkIdentity networkIdentity;

	private void Start ()
	{
		networkIdentity = GetComponent<NetworkIdentity>();
		playerId = networkIdentity.netId.ToString();
		transform.name = "PlayerClient: " + playerId;
		//if(isServer)
		if (!hasAuthority)
			return;

		CmdSpawn(playerPrefab);
		// Spawn(playerPrefab);
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