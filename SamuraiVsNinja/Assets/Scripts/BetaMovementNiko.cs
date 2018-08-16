using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaMovementNiko : MonoBehaviour {

    public GameObject player;
    public float jumpPWR;
    Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	
	void FixedUpdate () {
	    if (Input.GetKeyDown(KeyCode.A)) {
            rb.velocity += new Vector2 (-1, 0);
        }
        if (Input.GetKey(KeyCode.D)) {
            rb.velocity += new Vector2(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            print("jumpp");
            rb.AddForce(Vector2.up * jumpPWR);
        }

	}
}
