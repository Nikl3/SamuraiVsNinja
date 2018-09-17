using UnityEngine;

public class OnigiriFloater : MonoBehaviour {
    private float degreesPerSecond = 15.0f;
    private float amplitude = 0.5f;
    private float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start()
    {        
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement object on Y-Axis.
        transform.Translate(new Vector2(Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down.
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}


