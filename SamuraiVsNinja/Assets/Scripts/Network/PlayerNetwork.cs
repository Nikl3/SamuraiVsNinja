using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 serverVelocity;
    private Vector2 predictedPosition;
    private float latency = 0f;
    private float latencySmoothing = 50f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //networkClient = new NetworkClient();
    }

    public override void OnStartAuthority()
    {
        //UIManager.Instance.UpdateNetwprkText("OnStartAuthority: " + gameObject.name);
        spriteRenderer.color = Color.blue;
    }

    private void Update()
    {
        //latency = networkClient.GetRTT();
        //UIManager.Instance.UpdateLatencyText(latency.ToString());

        if (hasAuthority == false)
        {
            predictedPosition = predictedPosition + (serverVelocity * Time.deltaTime);

            transform.position = Vector2.Lerp(transform.position, predictedPosition, Time.deltaTime * latencySmoothing);

            return;
        }

        AuthorityUpdate();
        CmdUpdateVelocity(serverVelocity, transform.position);
    }

    private void AuthorityUpdate()
    {
        //if (hasAuthority == false)
        //    return;
        // Listen for player input commands
        PlayerInput.Instance.UpdateLocalInputs();
    }

    [Command]
    private void CmdUpdateVelocity(Vector2 velocity, Vector2 position)
    {
        serverVelocity = velocity;
        transform.position = position;
        //UIManager.Instance.UpdateNetwprkText("CmdUpdateVelocity: " + "v: " + velocity + " " + "p: " + position);
        RpcUpdateVelocity(serverVelocity, transform.position);
    }

    [ClientRpc]
    private void RpcUpdateVelocity(Vector2 velocity, Vector2 position)
    {
        if (hasAuthority)
        {
            return;
        }

        serverVelocity = velocity;
        predictedPosition = position + (serverVelocity * (latency));
        //UIManager.Instance.UpdateNetwprkText("CmdUpdateVelocity: " + "v: " + serverVelocity + " " + "p: " + predictedPosition);
    }
}
