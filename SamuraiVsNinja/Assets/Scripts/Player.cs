using UnityEngine;

public class Player : MonoBehaviour {
    public SpriteRenderer bodySprite;
    public float movementForce = 2f;

    private Rigidbody2D rb;

    //default keyboard input values
    private string horizontalAxisName = "Horizontal", verticalAxisName = "Vertical";
    private string action1Button = "Action1";

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Color color, int inputIndex) {
        bodySprite.color = color;

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
