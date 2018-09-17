using UnityEngine;

public class OnigiriFloater : MonoBehaviour
{
    private readonly float degreesPerSecond = 15.0f;
    private readonly float amplitude = 0.5f;
    private readonly float frequency = 1f;

    // Position Storage Variables
    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();

    // Use this for initialization
    private void Start()
    {        
        posOffset = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // Movement object on Y-Axis.
        transform.Translate(new Vector2(Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down.
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}


