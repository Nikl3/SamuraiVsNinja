using System.Collections;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
	private Vector2 startPosition;
	private Vector2 destinationPosition;
	private float moveSpeed;
	
	private void Awake ()
	{
		startPosition = transform.position;
	}

	private void Start()
	{
		StartCoroutine(IChangeDirection()); ;
	}

	private void Update()
	{
		Vector2 currentPosition = transform.position;
		transform.position = Vector2.MoveTowards(currentPosition, destinationPosition, moveSpeed * Time.deltaTime);
	}

	private IEnumerator IChangeDirection()
	{
		Vector2 startPosition = new Vector2(this.startPosition.x, this.startPosition.y);
		destinationPosition = startPosition += new Vector2(Random.Range(-15, 15), Random.Range(-7, 20));
		moveSpeed = Random.Range(5, 15);

		yield return new WaitForSeconds(Random.Range(2, 5));

		StartCoroutine(IChangeDirection());
	}
}
