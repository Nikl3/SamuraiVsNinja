using UnityEngine;

public class DebugManager : Singelton<DebugManager>
{
    [SerializeField]
    private bool isDebugingMessages = false;

    [SerializeField]
    private bool isDebugingRaycasts = false;

    /// <summary>
    /// Message types: 1 = print, 2 = warning, 3 = error.
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="debugMessage"></param>
    public void DebugMessage(int messageType, string debugMessage)
    {
        if (isDebugingMessages)
        {
            switch (messageType)
            {
                case 1:
                    Debug.Log(debugMessage);
                    break;

                case 2:
                    Debug.LogWarning(debugMessage);
                    break;

                case 3:
                    Debug.LogError(debugMessage);
                    break;
            }
        }     
    }

    public void DrawRay(Vector2 rayStartPoint, Vector2 rayDirection,float rayLenght, Color rayColor = new Color())
    {
        if(isDebugingRaycasts)
        Debug.DrawRay(rayStartPoint, Vector2.right * rayLenght, Color.red);
    }
}
