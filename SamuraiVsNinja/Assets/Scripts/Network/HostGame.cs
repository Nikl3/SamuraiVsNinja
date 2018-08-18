using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 2;
    
    private string roomName;

    public void SetRoomName(string roomName)
    {
        this.roomName = roomName;
    }

    public void CreateRoom()
    {
        if(roomName != null && roomName != "")
        {
            Debug.Log("Creating room: " + roomName + " Room size: " + roomSize);
        }
    }
}
