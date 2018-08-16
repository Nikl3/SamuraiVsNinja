using UnityEngine;
using UnityEngine.Networking;

public class ClientManager : NetworkBehaviour
{
	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	[SyncVar(hook = "OnNameChanced")] // SyncVars are variables where if their value changes on the server, then all clients are automatically informed of the new value.
	private string clientName = "Anonymous";

	public bool IsLocalPlayer
	{
		get
		{
			return isLocalPlayer;
		}       
	}

	private void Start ()
	{
		if (!IsLocalPlayer)
			return;

		// Instantiate(playerPrefab);
		Debug.Log("Spawn personal 'player units'");
		CmdSpawnPlayerUnit();
	}

	private void Update()
	{
		if (!IsLocalPlayer)
			return;

		if (Input.GetKeyDown(KeyCode.N))
		{
			string name = "Player" + Random.Range(1, 8);
			Debug.Log("Sendin the server a request to change our name to: " + name);
			CmdChangeClientName(name);
		}
	}

	[Command]
	private void CmdSpawnPlayerUnit()
	{
		// We are on the server right now.
		GameObject go = Instantiate(playerPrefab);

		// Now "go" exists on the server, propagate it to all the clients
		// (and also wire up the NetworkIdentity)
		NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
	}

	[Command]
	private void CmdChangeClientName(string newName)
	{
		Debug.Log("CmdChangeClientName: " + newName);
		clientName = newName;

		// Tell all the clients what this player's new name is.
		//RpcChangeClientName(clientName);
	}
	
	private void OnNameChanced(string newName)
	{
		Debug.Log("OnNameChanced: old name: " + clientName + " new name: " + newName);

		// If we using a hook on a SyncVar, the our local value does not get automatically updated.
		clientName = newName;

		gameObject.name = "ClientManager [" + newName + "]";
	}
	
	// RPC functions only get executed on the clients.
	//[ClientRpc]
	//private void RpcChangeClientName(string newName)
	//{
	//	Debug.Log("RpcChangeClientName: We were asked to change the client name on a particular ClientManager object: " + newName);
	//	clientName = newName;
	//}
}
