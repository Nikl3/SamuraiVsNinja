using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
    #region VARIABLES

    private PlayerInput playerInput;
    private SpriteRenderer spriteRenderer;
    private Vector2 serverVelocity;
    private Vector2 predictedPosition;
    private float latency = 0f;
    private float latencySmoothing = 50f;

    #endregion VARIABLES

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnStartAuthority()
    {
        spriteRenderer.color = Color.blue;
    }
  
    private void Update()
    {
        if (hasAuthority == false)
        {
            predictedPosition = predictedPosition + (serverVelocity * Time.deltaTime);
            transform.position = Vector2.Lerp(transform.position, predictedPosition, Time.deltaTime * latencySmoothing);
            return;
        }

        latency = MainNetworkManager.Instance.client.GetRTT();
        UIManager.Instance.DebugText = latency.ToString();
        AuthorityUpdate();
        CmdUpdateVelocity(serverVelocity, transform.position);

    }

    private void AuthorityUpdate()
    {
        playerInput.UpdateLocalInputs();
    }
   
    #region SERVER_COMMAND_FUNCTIONS

    [Command]
    private void CmdUpdateVelocity(Vector2 velocity, Vector2 position)
    {
        serverVelocity = velocity;
        transform.position = position;
        RpcUpdateVelocity(serverVelocity, transform.position);
    }

    #endregion SERVER_COMMAND_FUNCTIONS

    #region CLIENT_FUNCTIONS

    [ClientRpc]
    private void RpcUpdateVelocity(Vector2 velocity, Vector2 position)
    {
        if (hasAuthority)
            return;

        serverVelocity = velocity;
        predictedPosition = position + (serverVelocity * (latency));
    }

    #endregion CLIENT_FUNCTIONS
}
