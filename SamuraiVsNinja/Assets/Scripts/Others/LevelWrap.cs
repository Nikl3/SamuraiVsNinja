using UnityEngine;

public class LevelWrap : MonoBehaviour {

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player")
			print("pelaaja osui reunaan");

		var prb = collision.gameObject.GetComponent<Rigidbody2D>();
		var oldPos = prb.transform.position;
		prb.transform.position = new Vector2(-transform.position.x, oldPos.y);
		// miten saada liike jatkumaan screenwrapin jälkeen
	}
}
