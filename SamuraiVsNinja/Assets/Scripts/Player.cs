using UnityEngine;

[System.Serializable]
public class PlayerStats {
    public int Coins;
    public int Health;
    public int PlayerID;
    public GameObject PlayerInfo;
    public PlayerStats(int ID, int coins = 0, int health = 10) {
        Coins = coins;
        Health = health;
        PlayerID = ID;
    }
} 

public class Player : MonoBehaviour {
    public SpriteRenderer bodySprite;
    public float movementForce = 2f;


    private Rigidbody2D rb;
    public PlayerStats ps;
    public GameObject PlayerInfoPrefab;
    public Transform PlayerInfoContainer;

    //default keyboard input values
    private string horizontalAxisName = "Horizontal", verticalAxisName = "Vertical";
    private string action1Button = "Action1";

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        PlayerInfoContainer = GameObject.Find("PlayerInfoContainer").transform;
    }

    public void Init(Color color, int inputIndex) {
        bodySprite.color = color;
        ps = new PlayerStats(inputIndex);
        ps.PlayerInfo = Instantiate(PlayerInfoPrefab);
        ps.PlayerInfo.transform.SetParent(PlayerInfoContainer);


        // index 0 uses default keyboard input axes
        if (inputIndex > 0)
        {
            horizontalAxisName  = "Horizontal_J" + inputIndex;
            verticalAxisName    = "Vertical_J" + inputIndex;
            action1Button       = "Action1_J" + inputIndex;
        }
    }

    public void Update(){
        //movement
        float horizontal = Input.GetAxis(horizontalAxisName);
        float vertical = Input.GetAxis(verticalAxisName);

        rb.velocity = new Vector2(horizontal, vertical).normalized * movementForce;

        //actions
        if (Input.GetButtonDown(action1Button)) {
            //?
        }
    }
}
